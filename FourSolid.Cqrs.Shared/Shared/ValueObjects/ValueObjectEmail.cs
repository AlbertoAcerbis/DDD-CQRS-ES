using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FourSolid.Shared.ValueObjects
{
    public abstract class ValueObjectEmail<T> where T : ValueObjectEmail<T>
    {
        private const string EmailPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public string Value { get; }

        protected ValueObjectEmail(string value)
        {
            if (!string.IsNullOrEmpty(value) && !new Regex(EmailPattern).IsMatch(value))
                throw new ArgumentNullException("Email Address is not valid!");

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

        public static bool operator ==(ValueObjectEmail<T> left, ValueObjectEmail<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObjectEmail<T> left, ValueObjectEmail<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.GetAttributesToIncludeInEqualityCheck()
                .Aggregate(17, (current, obj) => current * 31 + (obj?.GetHashCode() ?? 0));
        }

        public virtual string GetValue()
        {
            return this.Value;
        }

        public virtual bool IsValid()
        {
            return new Regex(EmailPattern).IsMatch(this.Value);
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