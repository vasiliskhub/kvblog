using Kvblog.Client.Razor.Utilities;

namespace Kvblog.Client.Razor.Tests;

[TestFixture]
public class BlogArticleUtilsTests
{
	[TestCase(null, "")]
	[TestCase("", "")]
	[TestCase("   ", "")]
	[TestCase("<p>No image here</p>", "")]
	[TestCase("<img src='image1.jpg' />", "image1.jpg")]
	[TestCase("<img alt='x' src=\"image2.png\" />", "image2.png")]
	[TestCase("<div><img src='img3.gif'></div>", "img3.gif")]
	[TestCase("<img src='a.jpg'><img src='b.jpg'>", "a.jpg")]
	public void GetFirstImageSrc_ReturnsExpected(string? html, string expected)
	{
		var result = BlogArticleUtils.GetFirstImageSrc(html);
		Assert.That(result, Is.EqualTo(expected));
	}

	[TestCase(null, 0)]
	[TestCase("", 1)]
	[TestCase("<p>word</p>", 1)]
	[TestCase("<p>one two three four five</p>", 1)]
	public void GetReadTime_ReturnsExpected(string? html, int expected)
	{
		var result = BlogArticleUtils.GetReadTime(html);
		Assert.That(result, Is.EqualTo(expected));
	}

	[Test]
	public void GetReadTime_ReturnsExpected_ForLongText()
	{
		var text = string.Join(" ", Enumerable.Repeat("word", 220));
		var html = $"<p>{text}</p>";
		var result = BlogArticleUtils.GetReadTime(html);
		Assert.That(result, Is.EqualTo(3));
	}

	[Test]
	public void GetPostedDate_ReturnsFormattedDate()
	{
		var date = new DateTime(2024, 6, 8);
		var result = BlogArticleUtils.GetPostedDate(date);
		Assert.That(result, Is.EqualTo("08 Jun 24"));
	}

	[Test]
	public void GetPostedDate_ReturnsEmpty_WhenNull()
	{
		var result = BlogArticleUtils.GetPostedDate(null);
		Assert.That(result, Is.EqualTo(string.Empty));
	}
}