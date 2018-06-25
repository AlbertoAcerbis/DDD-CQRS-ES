using System;

namespace FourSolid.Shared.ValueObjects
{
    public sealed class DataPrevistaConsegna : ValueObjectDateTime<DataPrevistaConsegna>
    {
        public DataPrevistaConsegna(DateTime value) : base(value)
        {
        }
    }
}