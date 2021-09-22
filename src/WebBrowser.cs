using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using AleungcMailCollector.Interfaces;

namespace AleungcMailCollector
{
    /// <summary>
    /// WebBrowser class
    /// 
    /// Implements IWebBrowser interface. Will handle all stuff related
    /// to html pages opening.
    /// </summary>
    class WebBrowser : IWebBrowser
    {
        private WebClient _client;
        public string GetHtml(string url)
        {
            try
            {
                _client = new WebClient();
                _client.Headers.Add("User-Agent", "C# console program");
                string content = _client.DownloadString(url);
                return content;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exception in WebBrowser.GetHtml(" + url + ") : " + e.Message);
                Console.ResetColor();
                return null;
            }
        }
    }
}
