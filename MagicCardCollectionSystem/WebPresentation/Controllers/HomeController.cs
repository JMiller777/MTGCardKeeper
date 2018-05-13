using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebPresentation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Welcome to the MTG collection keeper web app!";

            return View();
        }

        [Authorize(Roles="Admin")]
        public ActionResult Contact()
        {
            ViewBag.Message = User.Identity.Name.ToString() + "'s contact page.";

            return View();
        }
    }
}