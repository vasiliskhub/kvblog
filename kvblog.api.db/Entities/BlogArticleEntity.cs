using System.ComponentModel.DataAnnotations;

namespace Kvblog.Api.Db.Entities
{
    public class BlogArticleEntity
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(5000)]
        public string Title { get; set; }
        [MaxLength(5000)]
        public string Description { get; set; }
        [Required]
        public string Body { get; set; }
        [MaxLength(1000)]
        public string FeaturedImageUrl { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? Author { get; set; }
    }
}
