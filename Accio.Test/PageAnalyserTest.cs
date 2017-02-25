using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Accio.Test
{
    public class PageAnalyserTest
    {
        [Fact]
        public void TestTicketInformationPage()
        {
            var ticketInformationPageText = File.ReadAllText(@"Resources\TicketInformationPage.html");
            var result = new PageAnalyser().GetQueueLink(ticketInformationPageText);
            Assert.Equal("https://enta.queue-it.net/?c=enta&e=hpplay24a", result);
        }

        [Fact]
        public void TestLandingPage()
        {
            var landingPageText = File.ReadAllText(@"Resources\LandingPage.html");
            var result = new PageAnalyser().GetBookingPageLink(landingPageText);
            Assert.Equal("../EntaWebGateway/gateway.aspx?E=N&QL=S2728|RCAR1|VPAL|G~/WEBPAGES/EntaWebShow/ShowPerformance.aspx", result);
        }

        [Fact]
        public void TestExtractData()
        {
            var bookingPageText = File.ReadAllText(@"Resources\BookingPage.html");
            var result = new PageAnalyser().ExtractDataFromBookingPage(bookingPageText);
            Assert.StartsWith(@"15/02/2017 14:00:00;Part One\n14:00 ;206626;0;1;FL1|15/02/2017 19:30:00;Part Two\n19:30 ;206868", result);
            var performances = Performance.ParsePerformances(result).ToList();
            Assert.Equal(492, performances.Count);
            Assert.Equal(1, performances.First().Part);
        }
    }
}
