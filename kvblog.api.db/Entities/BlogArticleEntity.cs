using System.ComponentModel.DataAnnotations;

namespace Kvblog.Api.Db.Entities;

public partial class BlogArticleEntity
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(500)]
    public required string Title { get; set; }
    [MaxLength(5000)]
    public required string Description { get; set; }
    [Required]
    public required string Body { get; set; }
    [Required]
    public DateTime? DatePosted { get; set; }
    [Required]
    public DateTime? DateUpdated { get; set; }
    [MaxLength(500)]
    [Required]
    public string? Author { get; set; }
    [Required]
    [MaxLength(500)]
    public required string Slug { get; set; }

}
