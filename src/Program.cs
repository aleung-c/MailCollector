using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AleungcMailCollector;

namespace AleungcMailCollector
{
    class Program
    {
        static int Main(string[] args)
        {
            List<string>    emailList = new List<string>();
            WebCrawler      crawler = new WebCrawler();
            WebBrowser      browser = new WebBrowser();
            RunTests        tests = new RunTests();

            if (args.Length == 0)
            {
                Console.WriteLine("Please provide string argument, first argument will be the url, second the depth.");
                return 1;
            }
            else
            {
                if (args.Length == 1 && args[0] == "--tests")
                {
                    return (tests.Run());
                }
                else
                {
                    if (args.Length == 1) {
                        emailList = crawler.GetEmailsInPageAndChildPages(browser, args[0], 0);
                    }
                    else if (args.Length == 2) {
                        int depth = Int32.Parse(args[1]);
                        if (depth > 10)
                        {
                            Console.WriteLine("-Limiting depth to 10-");
                            depth = 10;
                        }
                        emailList = crawler.GetEmailsInPageAndChildPages(browser, args[0], depth);
                    }

                    Console.WriteLine("Collected mails:");
                    foreach (string mail in emailList) {
                        Console.WriteLine(mail);
                    }
                    return 0;
                }
            }
        }
    }
}
