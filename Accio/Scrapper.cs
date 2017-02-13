using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Accio
{
    public class Scrapper
    {
        private const string BasePageUrl = "https://harrypottertheplay.nimaxtheatres.com/hpcc/WEBPAGES/";
        private const string LandingPageUrl = "https://harrypottertheplay.nimaxtheatres.com/hpcc/WEBPAGES/EntaWebShow/ShowLanding.aspx";

        private readonly PageAnalyser _analyser = new PageAnalyser();

        public async Task<IEnumerable<Performance>> DownloadPerformances()
        {
            using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler, false))
            {                
                handler.UseCookies = true;
                handler.AllowAutoRedirect = true;

                var landingPageText = await client.GetStringAsync(LandingPageUrl);

                var bookingPageLink = _analyser.GetBookingPageLink(landingPageText);
                var bookingPageUrl = BasePageUrl + bookingPageLink.Replace("../", "");

                var bookingPageContent = await client.GetStringAsync(bookingPageUrl);
                var bookingData = _analyser.ExtractDataFromBookingPage(bookingPageContent);

                return Performance.ParsePerformances(bookingData);
            }
        }
    }
}
