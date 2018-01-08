using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SimplePlugin.Controllers
{
    public class SimpleController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Widget()
        {
            return PartialView();
        }

        public ActionResult SecondWidget()
        {
            return PartialView();
        }

        public ActionResult Metatags()
        {
            return PartialView();
        }

        public ActionResult FooterMessage()
        {
            return PartialView();
        }
    }
}
