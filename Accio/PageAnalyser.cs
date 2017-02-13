using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;

namespace Accio
{
    public class PageAnalyser
    {
        public string GetBookingPageLink(string landingPage)
        {
            var htmlPage = new HtmlParser().Parse(landingPage);
            return htmlPage.All
                           .OfType<IHtmlAnchorElement>()
                           .Where(a => a.Href.Contains("|RCAR1|"))
                           .Select(a => a.GetAttribute("href"))
                           .Single();
        }

        public string ExtractDataFromBookingPage(string bookingPage)
        {
            var htmlPage = new HtmlParser().Parse(bookingPage);
            return htmlPage.All
                           .OfType<IHtmlInputElement>()
                           .Where(i => i.Name == "ctl00$MainContentPlaceHolder$txtDates")
                           .Select(i => i.Value)
                           .Single();
        }
    }
}
