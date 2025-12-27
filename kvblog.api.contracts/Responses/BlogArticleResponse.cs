namespace Kvblog.Api.Contracts.Responses;

public class BlogArticleResponse
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Body { get; set; }
    public DateTime DatePosted { get; set; }
    public DateTime DateUpdated { get; set; }
    public required string Author { get; set; }
    public required string Slug { get; set; }
}
