using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using PlanyATH_Project.Models;

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
    }
}