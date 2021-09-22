# Aleung-c Mail Collector

### Intro:
A simple C# program to collect mail addresses from websites.
Implements a simple node research algorithm, something looking like A*.

It opens pages, download the HTML, and parse it to extract simple mail addresses.

## Specifications:
 - **Langage**: C#
 - **Plaform**: .Net Core 3.1.0
 - **IDE used**: Visual studio community 2019

### Installation and usage:
- Using Visual Studio: double-click ```MailCollector.sln```, press the green play button, it will run the test according to the args in ```Properties/launchSettings.json```.
- Using shell: ```dotnet run --tests``` for launching tests, or ```dotnet run [PATH] [DEPTH]```, PATH being the path to the file you want to open, and DEPTH the
depth you want to explore.
- Working examples : 

```dotnet run http://www.csszengarden.com/ 1```

```dotnet run ./TestCases/ProvidedTest/index.html 3```

### Minimum code example:

```
        using System.Collections.Generic;
        using AleungcMailCollector;

        static void Main(string[] args)
        {
            List<string> emailList = new List<string>();

            WebCrawler crawler = new WebCrawler();
            WebBrowser browser = new WebBrowser();

            emailList = crawler.GetEmailsInPageAndChildPages(browser, "PATH_TO_PAGE"), 0);
        }
```
