using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using PlanyATH_Project.Models;
using Ical.Net.Interfaces;
using Ical.Net;
using Ical.Net.DataTypes;
using System;
using Ical.Net.Interfaces.Components;
using System.Collections;
using System.IO;
using System.Web.Services;

namespace PlanyATH_Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string searchQuery)
        {
            IEnumerable<DataModelView> resultlist;

            if (searchQuery != null)
            {
                resultlist = DataModelView.GetDataModelList().Where(p => p.Name.Contains(searchQuery)  || searchQuery == p.Name + " " + p.FileName).ToArray();

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

        protected CalendarCollection _Calendars;
        //protected string _CalendarAbsPath = @"C:\ICSFiles\L412.ics";

        [WebMethod]
        public ActionResult ICSReader(string path)
        {
            //_CalendarAbsPath = path;
            ViewBag.NowEvent = GetNowsEvents(path);
            ViewBag.TodayEvent = GetTodaysEvents(path);
            ViewBag.UpcomingEvent = GetUpcomingEvents(path);

            return View("View");
        }

        protected IList<Occurrence> GetNowsEvents(string filepath)
        {
            Calendar.LoadFromFile(Path.Combine(filepath));

            return _Calendars.GetOccurrences<IEvent>(DateTime.Now, DateTime.Now).ToList();
        }

        protected IList<Occurrence> GetTodaysEvents(string filepath)
        {
            Calendar.LoadFromFile(Path.Combine(filepath));
            
            return _Calendars.GetOccurrences<IEvent>(DateTime.Today, DateTime.Today.AddDays(1)).ToList();
        }

        protected IList<Occurrence> GetUpcomingEvents(string filepath)
        {
            Calendar.LoadFromFile(Path.Combine(filepath));

            return _Calendars.GetOccurrences<IEvent>(DateTime.Today.AddDays(1), DateTime.Today.AddDays(7)).ToList();
        }
    }
}