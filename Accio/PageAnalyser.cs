using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;

namespace Accio
{
    public class PageAnalyser
    {
        public Uri GetReserveationPageLink(string landingPage)
        {
            var htmlPage = new HtmlParser().Parse(landingPage);

            throw new NotImplementedException();
        }
    }
}
