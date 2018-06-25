namespace FourSolid.Shared.ValueObjects
{
    public class AccountId : ValueObjectString<AccountId>
    {
        public AccountId(string value) : base(value)
        {
        }
    }
}