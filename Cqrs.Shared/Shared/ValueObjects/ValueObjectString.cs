using System;
using System.Collections.Generic;
using System.Linq;

namespace FourSolid.Shared.ValueObjects
{
    public abstract class ValueObjectString<T> where T : ValueObjectString<T>
    {
        public string Value { get; }

        protected ValueObjectString(string value)
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

        public static bool operator ==(ValueObjectString<T> left, ValueObjectString<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObjectString<T> left, ValueObjectString<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.GetAttributesToIncludeInEqualityCheck()
                .Aggregate(17, (current, obj) => current * 31 + (obj?.GetHashCode() ?? 0));
        }

        public virtual Guid GetGuid()
        {
            Guid.TryParse(this.Value, out var valueGuid);
            return valueGuid;
        }

        public virtual string GetValue()
        {
            return this.Value;
        }

        public virtual bool IsValid()
        {
            return !string.IsNullOrEmpty(this.Value);
        }

        public virtual void ChkIsValid(string message = "")
        {
            if (this.IsValid())
                return;

            if (string.IsNullOrEmpty(message))
                message = $"{nameof(this.Value)} is Required!";
            //throw new ArgumentNullException(nameof(this.Value), message);
            throw new Exception(message);
        }
    }
}