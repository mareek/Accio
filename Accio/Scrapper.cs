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
        private const string TicketInformationPageUrl = "https://www.harrypottertheplay.com/ticket-information/";
        private const string BasePageUrl = "https://harrypottertheplay.nimaxtheatres.com/hpcc/WEBPAGES/";
        private const string LandingPageUrl = "https://harrypottertheplay.nimaxtheatres.com/hpcc/WEBPAGES/EntaWebShow/ShowLanding.aspx";

        private readonly PageAnalyser _analyser = new PageAnalyser();
        private WebBrowser _browser;

        public Scrapper(WebBrowser browser)
        {
            _browser = browser;
        }

        public async Task<IEnumerable<Performance>> DownloadPerformances()
        {
            var landingPageText = await _browser.DownloadPage(LandingPageUrl);

            var bookingPageLink = _analyser.GetBookingPageLink(landingPageText);
            var bookingPageUrl = BasePageUrl + bookingPageLink.Replace("../", "");

            var bookingPageContent = await _browser.DownloadPage(bookingPageUrl);
            var bookingData = _analyser.ExtractDataFromBookingPage(bookingPageContent);

            return Performance.ParsePerformances(bookingData);
        }
    }
}
