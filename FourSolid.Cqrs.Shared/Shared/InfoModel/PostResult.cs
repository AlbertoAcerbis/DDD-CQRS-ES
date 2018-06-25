namespace FourSolid.Shared.InfoModel
{
    public class PostResult
    {
        public string Uri { get; }
        public string Id { get; }

        public PostResult(string uri, string id)
        {
            this.Uri = uri;
            this.Id = id;
        }
    }
}