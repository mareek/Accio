using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Accio
{
    internal static class WebBrowserExtensions
    {
        public static async Task<string> DownloadPageAsync(this WebBrowser browser, string url)
        {
            browser.SilenceBrowser();
            await browser.GoToPageAsync(url);

            dynamic document = browser.Document;
            return document.documentElement.InnerHtml;
        }

        public static async Task GoToPageAsync(this WebBrowser browser, string url)
        {
            var resultGenerator = new TaskCompletionSource<bool>();
            LoadCompletedEventHandler loadCompletedEvent = (_, __) => resultGenerator.SetResult(true);

            browser.LoadCompleted += loadCompletedEvent;
            browser.Navigate(url);
            await resultGenerator.Task;
            browser.LoadCompleted -= loadCompletedEvent;
        }

        private static void SilenceBrowser(this WebBrowser browser)
        {
            dynamic activeX = typeof(WebBrowser).InvokeMember("ActiveXInstance",
                BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, browser, new object[0]);
            if (!activeX.Silent)
            {
                activeX.Silent = true;
            }
        }
    }
}
