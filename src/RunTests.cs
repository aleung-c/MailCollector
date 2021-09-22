using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AleungcMailCollector;

namespace AleungcMailCollector
{
    class RunTests
    {
        private static string GetThisFilePath([CallerFilePath] string path = null)
        {
            return path;
        }

        public int Run()
        {
            // --- Setup
            List<string> emailList = new List<string>();
            WebCrawler crawler = new WebCrawler();
            WebBrowser browser = new WebBrowser();

            string pathToTests = GetThisFilePath();
            pathToTests = Path.GetDirectoryName(pathToTests);
            pathToTests = Path.GetDirectoryName(pathToTests);
            pathToTests = Path.Combine(pathToTests, "TestCases");

            // Test executions
            String TestName = "Test 1 - Provided Test 1";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, Path.Combine(pathToTests, "ProvidedTest\\index.html"), 0);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Contains("nullepart@mozilla.org")) {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            TestName = "Test 2 - Provided Test 2";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, Path.Combine(pathToTests, "ProvidedTest\\index.html"), 1);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Contains("nullepart@mozilla.org")
                && emailList.Contains("ailleurs@mozilla.org"))
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            TestName = "Test 3 - Provided Test 3";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, Path.Combine(pathToTests, "ProvidedTest\\index.html"), 2);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Contains("nullepart@mozilla.org")
                && emailList.Contains("ailleurs@mozilla.org")
                && emailList.Contains("loin@mozilla.org"))
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            TestName = "Test 4 - Provided Test with NO mails in files - depth 0";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, Path.Combine(pathToTests, "ProvidedTestWithNoMail\\index.html"), 0);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Count == 0)
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            TestName = "Test 5 - Provided Test with NO mails in files - depth 1";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, Path.Combine(pathToTests, "ProvidedTestWithNoMail\\index.html"), 1);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Count == 0)
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            TestName = "Test 6 - Provided Test with NO mails in files - depth 3";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, Path.Combine(pathToTests, "ProvidedTestWithNoMail\\index.html"), 3);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Count == 0)
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            TestName = "Test 6 - Provided Test - depth 4 overflow";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, Path.Combine(pathToTests, "ProvidedTest\\index.html"), 4);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Contains("nullepart@mozilla.org")
                && emailList.Contains("ailleurs@mozilla.org")
                && emailList.Contains("loin@mozilla.org"))
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            TestName = "Test 6 - Provided Test - depth 10 overflow";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, Path.Combine(pathToTests, "ProvidedTest\\index.html"), 10);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Contains("nullepart@mozilla.org")
                && emailList.Contains("ailleurs@mozilla.org")
                && emailList.Contains("loin@mozilla.org"))
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            TestName = "Test 7 - Provided Test with NO links in files - depth 0";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, Path.Combine(pathToTests, "ProvidedTestWithNoLinks\\index.html"), 0);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Contains("nullepart@mozilla.org"))
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }
            

            TestName = "Test 8 - Provided Test with NO links in files - depth 1";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, Path.Combine(pathToTests, "ProvidedTestWithNoLinks\\index.html"), 1);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Contains("nullepart@mozilla.org"))
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            TestName = "Test 9 - Empty files - depth 0";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, Path.Combine(pathToTests, "EmptyFile\\index.html"), 0);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Count == 0)
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            TestName = "Test 10 - Empty files - depth 1";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, Path.Combine(pathToTests, "EmptyFile\\index.html"), 1);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Count == 0)
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            TestName = "Test 11 - Provided test with Invalid mails - depth 3";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, Path.Combine(pathToTests, "ProvidedTestWithInvalidMail\\index.html"), 3);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Count == 0)
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            TestName = "Test 12 - Stress test with Online address http://www.csszengarden.com/  - depth 0";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, "http://www.csszengarden.com/", 0);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Count == 0)
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            TestName = "Test 13 - Stress test with Online address http://www.csszengarden.com/  - depth 1";
            PrintTitle(TestName);
            emailList = crawler.GetEmailsInPageAndChildPages(browser, "http://www.csszengarden.com/", 1);
            PrintLog("Results:");
            foreach (string mail in emailList) { PrintLog(mail); }
            if (emailList.Count != 0) // Lets call it success if it scoops anything...
            {
                PrintSuccess(TestName + " PASS\n");
            }
            else
            {
                PrintFail(TestName + " FAIL\n");
            }

            return 0;
        }

        public void PrintTitle(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        public void PrintLog(string msg)
        {
            Console.WriteLine(msg);
        }

        public void PrintSuccess(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        public void PrintFail(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
