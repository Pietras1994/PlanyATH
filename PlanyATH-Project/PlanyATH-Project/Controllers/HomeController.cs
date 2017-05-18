﻿using System.Web.Mvc;
using Db4objects.Db4o;
using RestSharp;
using RestSharp.Extensions;
using HtmlAgilityPack;
using System.Linq;
using PlanyATH_Project.Models;
using System.Collections.Generic;

namespace PlanyATH_Project.Controllers
{
    public class HomeController : Controller
    {
        IObjectContainer db;
       public static string filename= "DataBase.data";
        public void DbInit()
        {
            db = Db4oEmbedded.OpenFile(filename);
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

            using (IObjectContainer db = Db4oEmbedded.OpenFile(filename))
            {
               

                string html = @"C:\Program Files (x86)\IIS Express\temp.html";
                HtmlDocument doc = new HtmlDocument();
                doc.Load(html);

                var nodes = doc.DocumentNode.Descendants().Where(n => n.Name == "a").Select(n => new { Name = n.InnerText, value = n.Attributes[0].Value }).ToList();

                foreach (var item in nodes)
                {

                    DataModel dm = new DataModel();
                    dm.Name = item.Name;
                    dm.Link = "http://plany.ath.bielsko.pl/" + item.value;

                    db.Store(dm);

                }
                db.Commit();
            }

        }
    


        //public ActionResult Index(string searchQuery)
        //{

        //    IEnumerable<DataModel> resultlist;


        //    if (searchQuery != null)
        //    {
        //        // zapytanie Linq, ktore bedzie filtrowalo dane ( w naszym przypadku listę osob) na podstawie wprwadzonego searchQuery
        //        resultlist = DataModel.GetDataModelList().Where(p => p.Name.Contains(searchQuery)).ToArray();

        //    }
        //    else // Jezeli zapytanie bedzie puste zwrocimy cala tablice naszych danych
        //        resultlist = DataModel.GetDataModelList().ToArray();

        //    if (Request.IsAjaxRequest()) // jezeli zapytanie bedzie typu ajax zwracamy widok czastkowy - w naszym przypadku bedzie tam wyswietlana lista , gdzie musimy rowniez przekazac obiekt
        //    {
        //        return PartialView("_PersonList", resultlist);
        //    }

        //    return View();
        //}
        public ActionResult PersonSuggestion(string term)
        {
            var personList = DataModel.GetDataModelList().Where(p => p.Name.Contains(term));
            return Json(personList, JsonRequestBehavior.AllowGet);
        }
    }
}