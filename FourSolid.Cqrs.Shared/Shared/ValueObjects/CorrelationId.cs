namespace FourSolid.Shared.ValueObjects
{
    public class CorrelationId : DomainIdBase<CorrelationId>
    {
        public CorrelationId(string value) : base(value)
        {
        }
    }
}