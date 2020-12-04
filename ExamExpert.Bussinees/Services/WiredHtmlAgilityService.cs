using ExamExpert.Bussinees.Interfaces;
using ExamExpert.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ExamExpert.Bussinees.Services
{
    public class WiredHtmlAgilityService : IWiredHtmlAgilityService
    {
        public List<WiredTypingsListVM> GetLastFiveTypings()
        {
            var wiredTypingsVM = new List<WiredTypingsListVM>();
            var countOfAddedMostRecentTypings = 0;

            var client = new HttpClient();
            var downloadedString = client.GetStringAsync("https://www.wired.com/most-recent/").Result;
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(downloadedString);

            var parentNodes = doc.DocumentNode.SelectNodes("//div[@class=\"archive-item-component__info\"]");

            if (parentNodes != null)
            {
                foreach (var parent in parentNodes)
                {
                    var h2Node = parent.ChildNodes["a"].ChildNodes["h2"].InnerHtml;
                    var href = parent.ChildNodes["a"].Attributes["href"].Value;

                    var downloadedContent = client.GetStringAsync("https://www.wired.com" + href).Result;
                    var docContent = new HtmlAgilityPack.HtmlDocument();
                    docContent.LoadHtml(downloadedContent);
                    var elementP = docContent.DocumentNode.SelectNodes("//p")[5];

                    wiredTypingsVM.Add(new WiredTypingsListVM { TypingTitle = h2Node, TypingContent = elementP.InnerText });
                    countOfAddedMostRecentTypings++;

                    if (countOfAddedMostRecentTypings == 5) { break; }
                }
            }

            return wiredTypingsVM;
        }
    }
}
