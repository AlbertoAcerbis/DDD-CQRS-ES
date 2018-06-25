using FourSolid.Shared.JsonFolder;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Shared.InfoModel
{
    public class AccountInfo
    {
        public readonly AccountId AccountId;
        public readonly AccountName AccountName;
        public readonly AccountRole AccountRole;

        public AccountInfo(AccountId accountId, AccountName accountName, AccountRole accountRole)
        {
            this.AccountId = accountId;
            this.AccountName = accountName;
            this.AccountRole = accountRole;
        }

        public AccountJson ToJson()
        {
            return new AccountJson
            {
                AccountId = this.AccountId.GetValue(),
                AccountName = this.AccountName.GetValue(),
                AccountRole = this.AccountRole.GetValue()
            };
        }
    }
}