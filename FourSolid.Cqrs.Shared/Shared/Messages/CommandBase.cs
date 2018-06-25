using System;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;
using Paramore.Brighter;

namespace FourSolid.Shared.Messages
{
    public abstract class CommandBase : Command
    {
        public Guid AggregateId { get; private set; }
        public CorrelationId CorrelationId { get; private set; }
        public AccountInfo Who { get; }
        public When When { get; }

        protected CommandBase(AccountInfo who, When when)
            : base(Guid.NewGuid())
        {
            this.Who = who;
            this.When = when;
        }

        protected void SetAggregateIdFromDomainId<T>(DomainIdBase<T> domainId) where T : DomainIdBase<T>
        {
            Guid.TryParse(domainId.Value, out var domainGuid);
            this.AggregateId = domainGuid;
        }

        protected void SetCorrelationId(CorrelationId correlationId)
        {
            this.CorrelationId = correlationId;
        }
    }
}