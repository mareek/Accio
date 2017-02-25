using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Accio.Test
{
    public class ScrapperTest
    {
        [Fact]
        public async Task TestWorkflow()
        {
            const string bookingPageUrl = "https://harrypottertheplay.nimaxtheatres.com/hpcc/WEBPAGES/EntaWebGateway/gateway.aspx?E=N&QL=S2728|RCAR1|VPAL|G~/WEBPAGES/EntaWebShow/ShowPerformance.aspx";
            var pageDownloaderFake = new PageDownloaderFake();
            pageDownloaderFake.SetPage(Scrapper.LandingPageUrl, File.ReadAllText(@"Resources\LandingPage.html"));
            pageDownloaderFake.SetPage(bookingPageUrl, File.ReadAllText(@"Resources\BookingPage.html"));
            var scrapper = new Scrapper(pageDownloaderFake);
            var performances = await scrapper.DownloadPerformances();
            Assert.NotEmpty(performances);
        }

        private class PageDownloaderFake : IPageDownloader
        {
            private readonly Dictionary<string, string> _pagesByUrl = new Dictionary<string, string>();

            public void SetPage(string url, string page) => _pagesByUrl[url] = page;

            public Task<string> DownloadPageAsync(string url) => Task.FromResult(_pagesByUrl[url]);
        }
    }
}
