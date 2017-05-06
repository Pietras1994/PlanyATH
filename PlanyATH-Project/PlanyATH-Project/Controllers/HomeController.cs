using System.Web.Mvc;
using Db4objects.Db4o;

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
    }
}