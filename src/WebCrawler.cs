using System;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using AleungcMailCollector.Interfaces;
using AleungcMailCollector.Exceptions;

namespace AleungcMailCollector
{
    /// <summary>
    ///  WebCrawler class.
    ///  
    /// Implements IAmTheTest interface. Will handle everything
    /// related to the crawler -> extract html code, email adress, and navigate
    /// through the pages until the maximum depth is reached.
    /// 
    /// Important note: it is NOT in recursive coding to avoid overhead
    /// on large depth values.
    /// 
    /// </summary>
    class WebCrawler : IAmTheTest
    {
        string  _htmlOutput = null;
        Regex   _tagRegex = new Regex("< ?a +href=\"([a-zA-Z\\d@./\\:-]+)\"");
        Regex   _mailRegex = new Regex("mailto:(.+)");

        // For mail validation.
        Regex   _mailValidationRegex = new Regex(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$",
                                                RegexOptions.IgnoreCase);

        /// <summary>
        /// The main method for the webcrawler.
        ///
        /// </summary>
        /// <param name="browser">Instance of WebBrowser class, will handle HTTP page opening.</param>
        /// <param name="url">The path to the page we want to open.</param>
        /// <param name="maximumDepth">The number of child pages to reach after the first page.</param>
        /// <returns>A list of emails extracted from pages, null if no mails were extracted</returns>
        public List<string> GetEmailsInPageAndChildPages(IWebBrowser browser, string url, int maximumDepth)
        {
            try
            {
                List<string>        emailList = new List<string>();

                HashSet<string>     pagesToCrawl = new HashSet<string>();
                HashSet<string>     crawledPages = new HashSet<string>();
                HashSet<string>     nextPagesToCrawl = new HashSet<string>();

                MatchCollection     matches;
                MatchCollection     innerMatches;

                string              capturedValue = "";
                bool                onlinePage = isUrlOnline(url); // Will be used to combine online addresses.
                
                pagesToCrawl.Add(url);
                while (maximumDepth > -1)
                {
                    foreach (string currentPage in pagesToCrawl)
                    {
                        _htmlOutput = browser.GetHtml(currentPage);
                        if (_htmlOutput == null) {
                            Console.WriteLine("Could not open url, continuing...");
                            continue;
                        }

                        // - Extracting HTML tags with 1st regex
                        matches = _tagRegex.Matches(_htmlOutput);
                        foreach (Match match in matches)
                        {
                            capturedValue = match.Groups[1].Value;
                            // - Extracting emails with second regex
                            innerMatches = _mailRegex.Matches(capturedValue);
                            if (innerMatches.Count != 0) {
                                ExtractEmails(emailList, innerMatches);
                            }
                            else {
                                // - Extracting next pages to crawl (considering non-mail captures as links)
                                ExtractPagesToCrawl(currentPage, crawledPages, nextPagesToCrawl, capturedValue, onlinePage);
                            }
                        }
                        crawledPages.Add(currentPage);
                    }
                    pagesToCrawl = new HashSet<string> (nextPagesToCrawl);
                    nextPagesToCrawl.Clear();
                    maximumDepth--;
                }
                return emailList;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exception in WebCrawler.GetEmailsInPageAndChildPages: " + e.Message);
                Console.ResetColor();
                return null;
            }
        }

        /// <summary>
        /// Small method to extract emails from the match, using regex capture groups.
        /// </summary>
        /// <param name="emailList">The list of mails address that will be filled.</param>
        /// <param name="match">The match resulting from the previously applied regex.</param>
        public void ExtractEmails(List<string> emailList, MatchCollection match)
        {
            string parsedMail;

            if (match[0].Groups[1] != null)
            {
                parsedMail = match[0].Groups[1].Value;
                // Email validation
                try
                {
                    if (IsValidEmail(parsedMail) && !emailList.Contains(parsedMail))
                    {
                        emailList.Add(parsedMail);
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Small method to validate email.
        /// I had to use a complex regex because of the test #11, which let things like
        /// nullepart@mozilla..org pass through when using EmailAddressAttribute().IsValid("youremailhere@test.test")
        /// See class attribute for the mail validation regex.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsValidEmail(string email)
        {
            return _mailValidationRegex.IsMatch(email);
        }

        /// <summary>
        /// A bit more complex, this method will take the values that did NOT match as mail
        /// address, and consider them button to access the next pages.
        /// 
        /// It is large because for the program to handle online adresses as well as local paths,
        /// and make the difference between local and absolute paths,
        /// the parsing must be done in various ways.
        /// </summary>
        /// <param name="currentPage">The current page path</param>
        /// <param name="crawledPages">the list of pages that we have already explored.</param>
        /// <param name="nextPagesToCrawl">the list of pages that will be considered new pages to explore.</param>
        /// <param name="capturedValue">The captured value from the previously executed regex.</param>
        /// <param name="onlinePage">If true, the path is an online adress, and must be treated as such.</param>
        public void ExtractPagesToCrawl(string currentPage,
                                        HashSet<string> crawledPages,
                                        HashSet<string> nextPagesToCrawl,
                                        string capturedValue, bool onlinePage)
        {
            string parsedPage = "";

            if (capturedValue[0] == '.' || capturedValue[0] == '\\' || capturedValue[0] == '/')
            { // Relative path
                if (onlinePage) // Online addresses must be combined differently.
                {
                    Uri result = null;
                    if (Uri.TryCreate(new Uri(currentPage), capturedValue, out result)) {
                        parsedPage = result.ToString();
                    }
                }
                else
                {
                    string parent = Path.GetDirectoryName(currentPage);
                    if (parent != null) {
                        parsedPage = Path.Combine(Path.GetDirectoryName(currentPage), capturedValue);
                    }
                    else {
                        parsedPage = Path.Combine(currentPage, capturedValue);
                    }
                }
            }
            else { // Absolute path
                parsedPage = capturedValue;
            }
            nextPagesToCrawl.Add(parsedPage);
        }

        /// <summary>
        /// Will be used to check if the url given is on a local path (C:/) or an
        /// online http address.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool isUrlOnline(string url)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
    }
}
