using FourSolid.Shared.ValueObjects;

namespace FourSolid.Shared.InfoModel
{
    public class CommandInfo : InfoBase
    {
        public readonly AccountInfo Who;
        public readonly When When;

        public CommandInfo(AccountInfo who, When when)
        {
            this.Who = who;
            this.When = when;
        }
    }
}