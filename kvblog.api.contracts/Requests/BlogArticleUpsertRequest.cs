using System.ComponentModel.DataAnnotations;

namespace Kvblog.Api.Contracts.Requests;

public class BlogArticleUpsertRequest
{
    [Required]
    [MaxLength(500)]
    public required string Title { get; set; }
    [MaxLength(5000)]
    public required string Description { get; set; }
    [Required]
    public required string Body { get; set; }
    public DateTime? DatePosted { get; set; }
    public DateTime? DateUpdated { get; set; }
    [MaxLength(500)]
    public string? Author { get; set; }
}
