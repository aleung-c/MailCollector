using System;
using System.Collections.Generic;
using System.Text;
using AleungcMailCollector.Interfaces;

namespace AleungcMailCollector.Interfaces
{
    interface IMailCollector
    {
        List<string> GetEmailsInPageAndChildPages(IWebBrowser browser, string url, int maximumDepth);
    }
}
