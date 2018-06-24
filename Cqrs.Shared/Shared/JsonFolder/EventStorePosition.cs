namespace FourSolid.Shared.JsonFolder
{
    public class EventStorePosition
    {
        public long CommitPosition { get; set; }
        public long PreparePosition { get; set; }
    }
}