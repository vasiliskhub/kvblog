using System.Text.RegularExpressions;

namespace Kvblog.Client.Razor.Utilities
{
        public static class BlogArticleUtils
        {
                public static string GetFirstImageSrc(string? htmlContent)
                {
                        if (string.IsNullOrWhiteSpace(htmlContent))
                        {
                                return string.Empty;
                        }

                        var match = Regex.Match(htmlContent, "<img[^>]*src=[\"']([^\"']+)[\"'][^>]*>", RegexOptions.IgnoreCase);

                        if (match.Success && match.Groups.Count > 1)
                        {
                                return match.Groups[1].Value;
                        }

                        return string.Empty;
                }

        public static int GetReadTime(string? htmlContent)
		{
			if (htmlContent == null)
			{
				return 0;
			}
			var textCount = RemoveHtmlTags(htmlContent).Split(' ').Length;
			const int wordsPerMinute = 100;
			int readTime = (int)Math.Ceiling((double)textCount / wordsPerMinute);
			return readTime;
		}

		public static string GetPostedDate(DateTime? dateTime)
		{
			return dateTime?.ToString("dd MMM yy") ?? string.Empty;
		}

		private static string RemoveHtmlTags(string htmlContent)
		{
			// Remove image tags
			var contentWithoutImages = Regex.Replace(htmlContent, "<img[^>]*>", string.Empty, RegexOptions.IgnoreCase);

			// Strip other HTML tags to leave plain text
			var contentWithoutHtmlTags = Regex.Replace(contentWithoutImages, "<.*?>", string.Empty);

			return contentWithoutHtmlTags;
		}
	}
}
