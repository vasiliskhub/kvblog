namespace Kvblog.Api.Contracts.Responses
{
    public class BlogArticleResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Author { get; set; }
		public string Slug { get; set; }
	}
}
