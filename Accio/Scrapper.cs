using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Accio
{
    public class Scrapper
    {
        private const string BasePageUrl = "https://harrypottertheplay.nimaxtheatres.com/hpcc/WEBPAGES/";
        public const string LandingPageUrl = "https://harrypottertheplay.nimaxtheatres.com/hpcc/WEBPAGES/EntaWebShow/ShowLanding.aspx";

        private readonly PageAnalyser _analyser = new PageAnalyser();
        private IPageDownloader _pageDownloader;

        public Scrapper(IPageDownloader pageDownloader)
        {
            _pageDownloader = pageDownloader;
        }

        public async Task<IEnumerable<Performance>> DownloadPerformances()
        {
            var landingPageText = await _pageDownloader.DownloadPageAsync(LandingPageUrl);

            var bookingPageLink = _analyser.GetBookingPageLink(landingPageText);
            var bookingPageUrl = BasePageUrl + bookingPageLink.Replace("../", "");

            var bookingPageContent = await _pageDownloader.DownloadPageAsync(bookingPageUrl);
            var bookingData = _analyser.ExtractDataFromBookingPage(bookingPageContent);

            return Performance.ParsePerformances(bookingData);
        }
    }
}
