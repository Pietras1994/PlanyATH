using HtmlAgilityPack;
using PlanyATH_Server.Models;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Topshelf;

namespace PlanyATH_Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            //CreateDatabase();
            //GetData();
            //ReadDataFromFile();
            GetFileICS();
        }

        public static string filename = "DataBase.data";

        public static void CreateDatabase()
        {
            using (var db = new PlanContext())
            {
                var f = db.PlanModel.ToList();
            }
        }

        public static void GetData()
        {
            var client = new RestClient("http://plany.ath.bielsko.pl/right_menu_result_plan.php");
            var request = new RestRequest(Method.POST);
            request.AddParameter("application/x-www-form-urlencoded", "search=plan&conductors=1&rooms=1&word=", ParameterType.RequestBody);
            client.DownloadData(request).SaveAs("temp.html");
            IRestResponse response = client.Execute(request);

        }
        public static void ReadDataFromFile()
        {
            using (var db = new PlanContext())
            {
                //List<PlanModel> plany = new List<Models.PlanModel>();
                string html = @"C:\Program Files (x86)\IIS Express\StartPage.html";
                HtmlDocument doc = new HtmlDocument();

                StreamReader reader = new StreamReader(html, Encoding.UTF8);
                doc.Load(reader);

                var nodes = doc.DocumentNode.Descendants().Where(n => n.Name == "a").Select(n => new { Name = n.InnerText, value = n.Attributes[0].Value }).ToList();

                foreach (var item in nodes)
                {
                    var t = new PlanModel
                    {
                        Name = item.Name,
                        Link = "http://plany.ath.bielsko.pl/" + item.value
                    };
                    db.PlanModel.Add(t);
                }

                db.SaveChanges();
            }
        }

        public static void GetFileICS()
        {
            string html = @"C:\Program Files (x86)\IIS Express\StartPage.html";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(html);

            //Uri uri = new Uri("http://plany.ath.bielsko.pl/plan.php?type=10&id=7821");
            //HtmlWeb web = new HtmlWeb();
            //HtmlDocument doc = web.Load(uri.AbsoluteUri);


            //var nodes = doc.DocumentNode.Descendants().Where(n => n.Name == "a" && n.InnerText == "plan.ics - dane z zajęciami dla kalendarzy MS Outlook, Kalendarz Google")
            //    .Select(n => new { value = n.Attributes[0].Value }).FirstOrDefault();
            //var nodes = doc.DocumentNode.SelectSingleNode("//*[@id=\"files\"]");
            //List<string> temp = new List<string>();
            //var item = doc.DocumentNode.SelectSingleNode(".//*[contains(@class,'data')]")
            //  .Descendants("a").FirstOrDefault().Attributes["href"].Value.ToList();
            var nodes = doc.DocumentNode.Descendants().Where(n => n.Name == "a").Select(n => new { Name = n.InnerText, value = n.Attributes[0].Value }).ToList();
            //var node = doc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("data"));

        }
    }
}

