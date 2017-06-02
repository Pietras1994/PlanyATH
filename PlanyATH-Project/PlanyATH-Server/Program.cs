using HtmlAgilityPack;
using PlanyATH_Server.Models;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Topshelf;

namespace PlanyATH_Server
{
    public class Program
    {
        static void Main(string[] args)
        {
        //    HostFactory.Run(z =>
        //    {
        //        z.Service<ServiceCore>(s =>
        //        {
        //            s.ConstructUsing(c => new ServiceCore());
        //            s.WhenStarted(c => c.Start());
        //            s.WhenStopped(c => c.Stop());
        //        });
        //        z.RunAsLocalSystem();
        //        z.StartAutomatically();
        //    });
        }

        public static void RunnAllFunction()
        {
            CreateDirecrory();
            CreateDatabase();
            GetData();
            ReadDataFromFile();
        }

        public static void CreateDirecrory()
        {
            Directory.CreateDirectory(@"C:\ICSFiles");
        }

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
                string html = @"C:\Program Files (x86)\IIS Express\StartPage.html";
                HtmlDocument doc = new HtmlDocument();

                StreamReader reader = new StreamReader(html, Encoding.UTF8);
                doc.Load(reader);

                var nodes = doc.DocumentNode.Descendants().Where(n => n.Name == "a").Select(n => new { Name = n.InnerText, value = n.Attributes[0].Value }).ToList();

                string tempLink;

                foreach (var item in nodes)
                {
                    tempLink = "http://plany.ath.bielsko.pl/" + item.value + "&winW=500&winH=300&loadBG=000000";

                    GetFile(tempLink, item.Name);

                    var t = new PlanModel
                    {
                        Name = item.Name,
                        Link = tempLink,
                        FileName = @"C:\ICSFiles\" + item.Name + ".ics"
                    };
                    db.PlanModel.Add(t);
                }
                db.SaveChanges();
            }
        }

        public static void GetFile(string link, string FileName)
        {

            Uri uri = new Uri(link);
            
            WebClient w = new WebClient();
            string s = w.DownloadString(uri);

            string tempString = FindLink(s);

            if (tempString != "")
            {
                downloadICS("http://plany.ath.bielsko.pl/" + tempString, FileName);
            }
            
        }


        public struct LinkItem
        {
            public string Href;
            public string Text;

            public override string ToString()
            {
                return Href + "\n\t" + Text;
            }
        }

        public static string FindLink(string file)
        {
            string temp = "";
            
            MatchCollection m1 = Regex.Matches(file, @"(<a.*?>.*?</a>)",
                RegexOptions.Singleline);
            
            foreach (Match m in m1)
            {
                string value = m.Groups[1].Value;
                LinkItem i = new LinkItem();
                
                Match m2 = Regex.Match(value, @"href=\""(.*?)\""",
                    RegexOptions.Singleline);
                if (m2.Success)
                {
                    i.Href = m2.Groups[1].Value;
                }
                
                string t = Regex.Replace(value, @"\s*<.*?>\s*", "",
                    RegexOptions.Singleline);
                i.Text = t;
                
                string a = "plan.ics";
                if (i.Text.Contains(a))
                {
                    temp = i.Href;
                }
            }
            return temp;
        }

        public static void downloadICS(string link, string FileName)
        {
            string tempPath = @"C:\ICSFiles\" + FileName + ".ics";

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(link, tempPath);
            }
        }
    }
}

