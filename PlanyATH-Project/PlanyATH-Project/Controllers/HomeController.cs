using System.Web.Mvc;
using Db4objects.Db4o;
using RestSharp;
using RestSharp.Extensions;
using HtmlAgilityPack;
using System.Linq;
using PlanyATH_Server.Models;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using PlanyATH_Project.Models;

namespace PlanyATH_Project.Controllers
{
    public class HomeController : Controller
    {
        public static string filename = "DataBase.data";

        public void GetData()
        {
            var client = new RestClient("http://plany.ath.bielsko.pl/right_menu_result_plan.php");
            var request = new RestRequest(Method.POST);
            //request.AddHeader("postman-token", "0edcb9f6-0f5b-6bb2-ae00-03d2644d7b1e");
            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "search=plan&conductors=1&rooms=1&word=", ParameterType.RequestBody);
            client.DownloadData(request).SaveAs("StartPage.html");
            IRestResponse response = client.Execute(request);

        }

        public void ReadDataFromFile()
        {

            using (var db = new DataModelContext())
            {
                string html = @"C:\Program Files (x86)\IIS Express\StartPage.html";
                HtmlDocument doc = new HtmlDocument();

                StreamReader reader = new StreamReader(html, Encoding.UTF8);          
                doc.Load(reader);

                var nodes = doc.DocumentNode.Descendants().Where(n => n.Name == "a").Select(n => new { Name = n.InnerText, value = n.Attributes[0].Value }).ToList();

                foreach (var item in nodes)
                {

                    DataModelView dm = new DataModelView();
                    dm.Name = item.Name;
                    dm.Link = "http://plany.ath.bielsko.pl/" + item.value;

                    //GetFile("http://plany.ath.bielsko.pl/" + item.value);

                    db.DataModel.Add(dm);
                }
                db.SaveChanges();
            }
        }

        //public void GetFile(string link)
        //{
        //    HtmlDocument doc = new HtmlDocument();

        //    //StreamReader reader = new StreamReader(link, Encoding.UTF8);
        //    doc.Load(link);

        //    var nodes = doc.DocumentNode.Descendants().Where(n => n.Name == "a").Select(n => new { Name = "plan.ics - dane z zajęciami dla kalendarzy MS Outlook, Kalendarz Google",
        //        value = n.Attributes[0].Value }).FirstOrDefault();

            
        //}

        public ActionResult Index(string searchQuery)
        {
            IEnumerable<DataModelView> resultlist;

            if (searchQuery != null)
            {
                // zapytanie Linq, ktore bedzie filtrowalo dane ( w naszym przypadku listę osob) na podstawie wprwadzonego searchQuery
                resultlist = DataModelView.GetDataModelList().Where(p => p.Name.Contains(searchQuery)  || searchQuery == p.Name + " " + p.Link).ToArray();

            }
            else 
                resultlist = DataModelView.GetDataModelList().ToArray();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ResultView", resultlist);
            }

            return View(resultlist);
        }
        public ActionResult PersonSuggestion(string term)
        {
            var personList = DataModelView.GetDataModelList().Where(p => p.Name.Contains(term));
            return Json(personList, JsonRequestBehavior.AllowGet);
        }
    }
}