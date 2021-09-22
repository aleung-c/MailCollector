using System;
using System.Collections.Generic;
using System.Text;

namespace AleungcMailCollector.Interfaces
{
    // Provided interface. Just use it.
    interface IWebBrowser
    {
        // Returns null if the url could not be visited.
        string GetHtml(string url);
    }
}
