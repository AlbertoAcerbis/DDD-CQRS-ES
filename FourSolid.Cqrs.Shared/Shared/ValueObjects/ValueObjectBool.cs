using System.Collections.Generic;
using System.Linq;

namespace FourSolid.Shared.ValueObjects
{
    public abstract class ValueObjectBool<T> where T : ValueObjectBool<T>
    {
        public bool Value { get; }

        protected ValueObjectBool(bool value)
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

        public static bool operator ==(ValueObjectBool<T> left, ValueObjectBool<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObjectBool<T> left, ValueObjectBool<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.GetAttributesToIncludeInEqualityCheck()
                .Aggregate(17, (current, obj) => current * 31 + (obj?.GetHashCode() ?? 0));
        }

        public virtual bool GetValue()
        {
            return this.Value;
        }
    }
}