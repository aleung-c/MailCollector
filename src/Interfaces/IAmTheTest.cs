using System;
using System.Collections.Generic;
using System.Text;
using AleungcMailCollector.Interfaces;

namespace AleungcMailCollector.Interfaces
{
    interface IAmTheTest
    {
        List<string> GetEmailsInPageAndChildPages(IWebBrowser browser, string url, int maximumDepth);
    }
}
