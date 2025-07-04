﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Kvblog.Api.Db.Entities
{
    public partial class BlogArticleEntity
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        [MaxLength(500)]
        public string Title { get; set; }
        [MaxLength(5000)]
        public string Description { get; set; }
        [Required]
        public string Body { get; set; }
		[Required]
		public DateTime? DatePosted { get; set; }
		[Required]
		public DateTime? DateUpdated { get; set; }
		[MaxLength(500)]
		[Required]
		public string? Author { get; set; }
		[Required]
		[MaxLength(500)]
		public string Slug { get; set; }

	}
}
