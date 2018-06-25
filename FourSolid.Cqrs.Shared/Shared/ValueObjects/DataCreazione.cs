using System;

namespace FourSolid.Shared.ValueObjects
{
    public class DataCreazione : ValueObjectDateTime<DataCreazione>
    {
        public DataCreazione(DateTime value) : base(value)
        {
        }
    }
}