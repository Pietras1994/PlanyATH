using System.Web.Mvc;
using Db4objects.Db4o;
using RestSharp;
using RestSharp.Extensions;
using System.IO;
using HtmlAgilityPack;
using System.Linq;
using PlanyATH_Project.Models;
using System.Xml.XPath;
using System;
using System.Collections.Generic;

namespace PlanyATH_Project.Controllers
{
    public class HomeController : Controller
    {
        IObjectContainer db;
        public void DbInit()
        {
            db = Db4oEmbedded.OpenFile("DataBase.data");
        }

        public ActionResult Index()
        {
            return View();
        }

        public void GetData()
        {
            var client = new RestClient("http://plany.ath.bielsko.pl/right_menu_result_plan.php");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "0edcb9f6-0f5b-6bb2-ae00-03d2644d7b1e");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "search=plan&conductors=1&rooms=1&word=", ParameterType.RequestBody);
            client.DownloadData(request).SaveAs("temp.html");
            IRestResponse response = client.Execute(request);

        }

        public void ReadDataFromFile()
        {
            string html = @"C:\Program Files (x86)\IIS Express\temp.html";
            //HtmlDocument htmlDoc = new HtmlDocument();
            //htmlDoc.LoadHtml(html);
            List<string> templist = new List<string>();

            HtmlDocument doc = new HtmlDocument();
            doc.Load(html);

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//a[@id='result_link']"))
            {
                Console.WriteLine("node:" + node.GetAttributeValue("href", null));
            }

            //AddDataToBase(url1, text1);


        }

        public void AddDataToBase(string link, string name)
        {
            DataModel dm = new DataModel();
            dm.Name = name;
            dm.Link = link;

            db.Store(dm);
            db.Commit();
        }
    }
}