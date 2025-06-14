using System.ComponentModel.DataAnnotations;

namespace Kvblog.Api.Contracts.Requests
{
    public class BlogArticleUpsertRequest
    {
        [Required]
        [MaxLength(500)]
        public string Title { get; set; }
        [MaxLength(5000)]
        public string Description { get; set; }
        [Required]
        public string Body { get; set; }
		public DateTime? DatePosted { get; set; }
		public DateTime? DateUpdated { get; set; }
		[MaxLength(500)]
		public string? Author { get; set; }
    }
}
