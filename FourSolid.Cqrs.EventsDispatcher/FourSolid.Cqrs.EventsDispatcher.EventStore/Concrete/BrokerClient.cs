using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FourSolid.Cqrs.EventsDispatcher.EventStore.Abstract;
using FourSolid.Cqrs.EventsDispatcher.EventStore.Configuration;
using FourSolid.Cqrs.EventsDispatcher.Logging.Abstracts;
using Newtonsoft.Json;
using Paramore.Brighter;
using Paramore.Brighter.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FourSolid.Cqrs.EventsDispatcher.EventStore.Concrete
{
    public class BrokerClient : IBrokerClient
    {
        private readonly RmqParameters _rmqParameters;
        private readonly IConnection _rmqConnection;
        private readonly IModel _rmqChannel;
        private EventingBasicConsumer _rmqConsumer;
        private readonly ILogService _logService;

        private static readonly string[] HeadersToReset =
        {
            HeaderNames.DELAY_MILLISECONDS,
            HeaderNames.MESSAGE_TYPE,
            HeaderNames.TOPIC,
            HeaderNames.HANDLED_COUNT,
            HeaderNames.DELIVERY_TAG,
            HeaderNames.CORRELATION_ID
        };

        public BrokerClient(RmqParameters rmqParameters, ILogService logService)
        {
            this._logService = logService;

            this._rmqParameters = rmqParameters;
            var factory = new ConnectionFactory
            {
                HostName = this._rmqParameters.HostName,
                UserName = this._rmqParameters.Username,
                Password = this._rmqParameters.Password,
                //Uri = this._rmqParameters.Uri
            };

            this._rmqConnection = factory.CreateConnection();
            this._rmqChannel = this._rmqConnection.CreateModel();

            //var timeToLive = this._authSettings.RmqParameters.TimeToLive * 360000;
            //var args = new Dictionary<string, object>
            //{
            //    { "x-message-ttl", timeToLive }
            //};
            //this._rmqChannel.QueueDeclare(this._rmqParameters.Exchange, false, false, false, args);
            this._rmqChannel.ExchangeDeclare(this._rmqParameters.Exchange, "topic");
        }

        public void Send(Message message, string routingKey)
        {
            this.PublishMessage(message, 0);
        }

        public void Send(dynamic body, string routingKey)
        {
            try
            {
                var message = MapToMessage(body, routingKey);
                this.Send(message, routingKey);
                Console.WriteLine($"Sent message for event {routingKey}");
            }
            catch (Exception ex)
            {
                this._logService.ErrorTrace("BokerClient.Send", ex);
            }
        }

        public void Receive()
        {
            if (this._rmqConsumer != null)
                return;

            this._rmqConsumer = new EventingBasicConsumer(this._rmqChannel);
            this._rmqConsumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                this._rmqChannel.BasicAck(ea.DeliveryTag, false);
            };

            this._rmqChannel.BasicQos(0, 1, false);
            var consumerTag = this._rmqChannel.BasicConsume(
                this._rmqParameters.Exchange,
                false,
                string.Empty,
                SetConsumerArguments(),
                this._rmqConsumer
            );

            this._rmqChannel.BasicCancel(consumerTag);
        }

        #region Helpers
        private void PublishMessage(Message message, int delayMilliseconds)
        {
            var messageId = message.Id;
            var deliveryTag = message.Header.Bag.ContainsKey(HeaderNames.DELIVERY_TAG) ? message.DeliveryTag.ToString() : null;

            var headers = new Dictionary<string, object>
            {
                { HeaderNames.MESSAGE_TYPE, message.Header.MessageType.ToString() },
                { HeaderNames.TOPIC, message.Header.Topic },
                { HeaderNames.HANDLED_COUNT, message.Header.HandledCount.ToString(CultureInfo.InvariantCulture) }
            };

            if (message.Header.CorrelationId != Guid.Empty)
                headers.Add(HeaderNames.CORRELATION_ID, message.Header.CorrelationId.ToString());

            message.Header.Bag.Each(header =>
            {
                if (!HeadersToReset.Any(htr => htr.Equals(header.Key))) headers.Add(header.Key, header.Value);
            });

            if (!string.IsNullOrEmpty(deliveryTag))
                headers.Add(HeaderNames.DELIVERY_TAG, deliveryTag);

            if (delayMilliseconds > 0)
                headers.Add(HeaderNames.DELAY_MILLISECONDS, delayMilliseconds);

            this._rmqChannel.BasicPublish(
                this._rmqParameters.Exchange,
                message.Header.Topic,
                false,
                CreateBasicProperties(messageId, message.Header.TimeStamp, message.Body.BodyType, message.Header.ContentType, headers),
                message.Body.Bytes);
        }

        private IBasicProperties CreateBasicProperties(Guid id, DateTime timeStamp, string type, string contentType, IDictionary<string, object> headers = null)
        {
            var basicProperties = this._rmqChannel.CreateBasicProperties();

            basicProperties.DeliveryMode = 2;   // 1 - Non Persistent; 2 - Persistent
            basicProperties.ContentType = contentType;
            basicProperties.Type = type;
            basicProperties.MessageId = id.ToString();
            basicProperties.Timestamp = new AmqpTimestamp(UnixTimestamp.GetUnixTimestampSeconds(timeStamp));
            basicProperties.Expiration = "36000000";

            if (headers != null && headers.Any())
                basicProperties.Headers = headers;

            return basicProperties;
        }

        private static Message MapToMessage(dynamic request, string routingKey)
        {
            var header = new MessageHeader(
                messageId: Guid.NewGuid(), 
                topic: routingKey, 
                messageType: MessageType.MT_EVENT);
            var body = new MessageBody(JsonConvert.SerializeObject(request));
            var message = new Message(header, body);
            return message;
        }

        private static Dictionary<string, object> SetConsumerArguments()
        {
            var arguments = new Dictionary<string, object>
            {
                {"x-cancel-on-ha-failover", true}
            };
            return arguments;
        }

        private static class UnixTimestamp
        {
            private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            public static DateTime DateTimeFromUnixTimestampSeconds(long seconds)
            {
                return UnixEpoch.AddSeconds(seconds);
            }

            public static long GetCurrentUnixTimestampSeconds()
            {
                return (long)(DateTime.UtcNow - UnixEpoch).TotalSeconds;
            }

            public static long GetUnixTimestampSeconds(DateTime dateTime)
            {
                return (long)(dateTime - UnixEpoch).TotalSeconds;
            }
        }
        #endregion

        #region Dispose
        private bool _disposed;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._rmqConnection?.Close();
                    this._rmqConnection?.Dispose();
                    this._rmqChannel?.Dispose();
                }
            }
            this._disposed = true;
        }
        #endregion
    }
}