using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using PlanyATH_Project.Models;
using Ical.Net.Interfaces;
using Ical.Net;
using Ical.Net.DataTypes;
using System;
using Ical.Net.Interfaces.Components;
using System.IO;
using PlanyATH_Project.ViewModels;

namespace PlanyATH_Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string searchQuery)
        {
            IEnumerable<DataModelView> resultlist;

            if (searchQuery != null)
            {
                resultlist = DataModelView.GetDataModelList().Where(p => p.Name.Contains(searchQuery) || searchQuery == p.Name + " " + p.FileName).ToArray();

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

        //  [HttpPost]
        public ActionResult ICSReader(string path)
        {

            var viewModel = new ResultViewViewModel
            {
                NowEvent = GetNowsEvents(path),
                TodayEvent = GetTodaysEvents(path),
                UpcomingEvent = GetUpcomingEvents(path)
            };
            return PartialView("View", viewModel);
        }

        protected List<string> GetNowsEvents(string filepath)
        {
            IICalendarCollection calendars = Calendar.LoadFromFile(Path.Combine(filepath));
            IList<Occurrence> occurrencesn = calendars.GetOccurrences(DateTime.Now.AddHours(-2), DateTime.Now.AddHours(-2)).ToList();
            List<string> Events = new List<string>();

            foreach (Occurrence occurrence in occurrencesn)
            {
                DateTime occurrenceTime = occurrence.Period.StartTime.AsSystemLocal;
                DateTime occurrenceTimee = occurrence.Period.EndTime.AsSystemLocal;
                IRecurringComponent rc = occurrence.Source as IRecurringComponent;
                if (rc != null)
                    Events.Add(rc.Summary + ": " + occurrenceTime.ToShortTimeString() + " - " + occurrenceTimee.ToShortTimeString());
            }

            return Events;
        }

        protected List<string> GetTodaysEvents(string filepath)
        {
            IICalendarCollection calendars = Calendar.LoadFromFile(Path.Combine(filepath));
            IList<Occurrence> occurrencesn = calendars.GetOccurrences(DateTime.Today, DateTime.Today.AddDays(1)).ToList();
            List<string> Events = new List<string>();

            foreach (Occurrence occurrence in occurrencesn)
            {
                DateTime occurrenceTime = occurrence.Period.StartTime.AsSystemLocal;
                DateTime occurrenceTimee = occurrence.Period.EndTime.AsSystemLocal;
                IRecurringComponent rc = occurrence.Source as IRecurringComponent;
                if (rc != null)
                    Events.Add(rc.Summary + ": " + occurrenceTime.ToShortTimeString() + " - " + occurrenceTimee.ToShortTimeString());
            }

            return Events;
        }

        protected List<string> GetUpcomingEvents(string filepath)
        {
            IICalendarCollection calendars = Calendar.LoadFromFile(Path.Combine(filepath));
            IList<Occurrence> occurrencesn = calendars.GetOccurrences(DateTime.Today.AddDays(1), DateTime.Today.AddDays(7)).ToList();
            List<string> Events = new List<string>();

            foreach (Occurrence occurrence in occurrencesn)
            {
                DateTime occurrenceTime = occurrence.Period.StartTime.AsSystemLocal;
                DateTime occurrenceTimee = occurrence.Period.EndTime.AsSystemLocal;
                IRecurringComponent rc = occurrence.Source as IRecurringComponent;
                if (rc != null)
                    Events.Add(rc.Summary + ": " + occurrenceTime.ToShortTimeString() + " - " + occurrenceTimee.ToShortTimeString());
            }

            return Events;
        }
    }
}