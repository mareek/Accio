using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accio
{
    public interface IPageDownloader
    {
        Task<string> DownloadPageAsync(string url);
    }
}
