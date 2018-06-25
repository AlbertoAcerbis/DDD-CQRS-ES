using System;

namespace FourSolid.Shared.ValueObjects
{
    public class When : ValueObjectDateTime<When>
    {
        public When(DateTime value) : base(value)
        {
        }
    }
}