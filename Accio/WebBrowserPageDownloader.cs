using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Accio
{
    public class WebBrowserPageDownloader : IPageDownloader
    {
        private WebBrowser _browser;

        public WebBrowserPageDownloader(WebBrowser browser)
        {
            _browser = browser;
        }

        public async Task<string> DownloadPageAsync(string url) => await _browser.DownloadPageAsync(url);
    }
}
