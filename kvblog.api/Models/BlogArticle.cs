namespace Kvblog.Api.Models
{
    public class BlogArticle
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string FeaturedImageUrl { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? Author { get; set; }
    }
}
