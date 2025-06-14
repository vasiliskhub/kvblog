using System.ComponentModel.DataAnnotations;

namespace Kvblog.Client.Razor.ViewModels
{
    public class BlogArticle
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string? Title { get; set; }
        [Required]
        [MaxLength(5000)]
        public string? Description { get; set; }
        [Required]
        public string? Body { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateUpdated { get; set; }
		[MaxLength(500)]
		public string? Author { get; set; }
        public string? Slug { get; set; }
	}
}
