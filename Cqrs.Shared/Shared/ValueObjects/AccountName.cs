namespace FourSolid.Shared.ValueObjects
{
    public class AccountName : ValueObjectString<AccountName>
    {
        public AccountName(string value) : base(value)
        {
        }
    }
}