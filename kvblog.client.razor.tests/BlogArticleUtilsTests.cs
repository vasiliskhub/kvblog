using Kvblog.Client.Razor.Utilities;

namespace Kvblog.Client.Razor.Tests;

[TestFixture]
public class BlogArticleUtilsTests
{
    [Test]
    public void GetFirstImageSrc_ReturnsSrc_WhenImageExists()
    {
        var html = "<p>text</p><img src=\"/img/pic.jpg\" alt=\"a\" />";
        var result = BlogArticleUtils.GetFirstImageSrc(html);

        Assert.That(result, Is.EqualTo("/img/pic.jpg"));
    }

    [Test]
    public void GetFirstImageSrc_ReturnsEmpty_WhenNoImage()
    {
        var html = "<p>No image here</p>";
        var result = BlogArticleUtils.GetFirstImageSrc(html);

        Assert.That(result, Is.EqualTo(string.Empty));
    }
}
