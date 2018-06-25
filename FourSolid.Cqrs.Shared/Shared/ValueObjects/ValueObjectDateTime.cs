using System;
using System.Collections.Generic;
using System.Linq;

namespace FourSolid.Shared.ValueObjects
{
    public abstract class ValueObjectDateTime<T> where T : ValueObjectDateTime<T>
    {
        public DateTime Value { get; }

        protected ValueObjectDateTime(DateTime value)
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

        public static bool operator ==(ValueObjectDateTime<T> left, ValueObjectDateTime<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObjectDateTime<T> left, ValueObjectDateTime<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.GetAttributesToIncludeInEqualityCheck()
                .Aggregate(17, (current, obj) => current * 31 + (obj?.GetHashCode() ?? 0));
        }

        public virtual DateTime GetValue()
        {
            return this.Value;
        }

        public static DateTime GetMaxDateTime()
        {
            return DateTime.MaxValue;
        }

        public static DateTime GetMinDateTime()
        {
            return DateTime.MinValue;
        }

        public static DateTime GetUtcDateTime()
        {
            return DateTime.UtcNow;
        }

        /// <summary>
        /// Chk if Value is at least equal, or minor, of UtcNow.
        /// </summary>
        /// <param name="message"></param>
        public virtual void ChkIsValid(string message = "")
        {
            if (this.Value <= DateTime.UtcNow)
                return;

            if (string.IsNullOrEmpty(message))
                message = $"{nameof(this.Value)} is Invalid!";
            throw new ArgumentNullException(nameof(this.Value), message);
        }
    }
}