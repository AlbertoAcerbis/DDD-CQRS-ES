using System;
using System.Collections.Generic;
using System.Linq;

namespace FourSolid.Shared.ValueObjects
{
    public abstract class DomainIdBase<T> where T : DomainIdBase<T>
    {
        public string Value { get; }

        protected DomainIdBase(string value)
        {
            if (string.IsNullOrEmpty(value))
                value = Guid.NewGuid().ToString("N");

            this.Value = value;
        }

        protected virtual IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>();
        }

        public override bool Equals(object other)
        {
            return this.Equals(other as T);
        }

        public bool Equals(T other)
        {
            if (other == null) return false;

            return
                this.GetAttributesToIncludeInEqualityCheck()
                    .SequenceEqual(other.GetAttributesToIncludeInEqualityCheck());
        }

        public static bool operator ==(DomainIdBase<T> left, DomainIdBase<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DomainIdBase<T> left, DomainIdBase<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.GetAttributesToIncludeInEqualityCheck()
                .Aggregate(17, (current, obj) => current * 31 + (obj?.GetHashCode() ?? 0));
        }

        public Guid GetValueGuid()
        {
            Guid.TryParse(this.Value, out var valueGuid);
            return valueGuid;
        }

        public static string GetEmptyValue()
        {
            return Guid.Empty.ToString("N");
        }

        public string GetValue()
        {
            return this.Value;
        }

        public virtual bool IsValid()
        {
            return !string.IsNullOrEmpty(this.Value) && this.GetValueGuid() != Guid.Empty;
        }

        public virtual void ChkIsValid(string message = "")
        {
            if (this.IsValid())
                return;

            if (string.IsNullOrEmpty(message))
                message = $"{nameof(this.Value)} is Required!";
            throw new ArgumentNullException(nameof(this.Value), message);
        }
    }
}