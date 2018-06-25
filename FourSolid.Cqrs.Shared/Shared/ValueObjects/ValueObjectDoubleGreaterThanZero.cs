using System;
using System.Collections.Generic;
using System.Linq;

namespace FourSolid.Shared.ValueObjects
{
    public abstract class ValueObjectDoubleGreaterThanZero<T> where T : ValueObjectDoubleGreaterThanZero<T>
    {
        public double Value { get; }

        protected ValueObjectDoubleGreaterThanZero(double value)
        {
            this.Value = value;
        }

        public override bool Equals(object other)
        {
            return this.Equals(other as T);
        }

        protected virtual IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>();
        }

        public bool Equals(T other)
        {
            if (other == null) return false;

            return
                this.GetAttributesToIncludeInEqualityCheck()
                    .SequenceEqual(other.GetAttributesToIncludeInEqualityCheck());
        }

        public static bool operator ==(ValueObjectDoubleGreaterThanZero<T> left, ValueObjectDoubleGreaterThanZero<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObjectDoubleGreaterThanZero<T> left, ValueObjectDoubleGreaterThanZero<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.GetAttributesToIncludeInEqualityCheck()
                .Aggregate(17, (current, obj) => current * 31 + (obj?.GetHashCode() ?? 0));
        }

        public virtual double GetValue()
        {
            return this.Value;
        }

        public virtual bool IsValid()
        {
            return this.Value > 0;
        }

        public virtual void ChkIsValid(string message)
        {
            if (this.IsValid())
                return;

            if (string.IsNullOrEmpty(message))
                message = $"{nameof(this.Value)} is Required!";
            throw new ArgumentNullException(nameof(this.Value), message);
        }
    }
}