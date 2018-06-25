using Paramore.Brighter;

namespace FourSolid.Cqrs.EventsDispatcher.EventStore.Abstract
{
    /// <summary>
    /// Class HeaderNames.{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
    /// </summary>
    public class HeaderNames
    {
        /// <summary>
        /// The messag e_ type{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public const string MESSAGE_TYPE = "MessageType";
        /// <summary>
        /// The messag e_ identifier{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public const string MESSAGE_ID = "MessageId";
        /// <summary>
        /// The correlation id
        /// </summary>
        public const string CORRELATION_ID = "CorrelationId";
        /// <summary>
        /// The topic{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public const string TOPIC = "Topic";
        /// <summary>
        /// The handle d_ count{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public const string HANDLED_COUNT = "HandledCount";
        /// <summary>
        /// The milliseconds to delay the message by (requires plugin rabbitmq_delayed_message_exchange)
        /// </summary>
        public const string DELAY_MILLISECONDS = "x-delay";
        /// <summary>
        /// The milliseconds the message was instructed to be delayed for (sent as negative) (requires plugin rabbitmq_delayed_message_exchange)
        /// </summary>
        public const string DELAYED_MILLISECONDS = "x-delay";
        /// <summary>
        /// Indicates the original id of this message given a historic scenario (e.g. re-queueuing).
        /// </summary>
        public const string ORIGINAL_MESSAGE_ID = Message.OriginalMessageIdHeaderName;
        /// <summary>
        /// Tag used to identify this message in the sequence against its Id (used to perform multiple ack against Id upto Tag).
        /// </summary>
        public const string DELIVERY_TAG = Message.DeliveryTagHeaderName;
    }
}