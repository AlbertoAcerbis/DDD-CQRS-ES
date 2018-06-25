using System;

namespace FourSolid.Shared.ValueObjects
{
    public sealed class DataInserimento : ValueObjectDateTime<DataInserimento>
    {
        public DataInserimento(DateTime value) : base(value)
        {
        }
    }
}