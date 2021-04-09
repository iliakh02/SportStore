using SportStore.WebUI.Interfaces;
using SportStore.WebUI.Services;
using Xunit;

namespace SportStore.Tests
{
    public class UrlServiceTests
    {
        IUrlService urlService;
        public UrlServiceTests()
        {
            urlService = new UrlService();
        }
        [Theory]
        [InlineData(4, "page=4", "Test", @"\Test?page=4")]
        [InlineData(1, "page=4", "Test", @"\Test?page=3")]
        public void ReditectUrlForDelete(int pageSize, string queries, string controller, string expectedUrl)
        {
            var result = urlService.ReditectUrlForDelete(pageSize, queries, controller);

            Assert.Equal(result, expectedUrl);
        }
    }
}
