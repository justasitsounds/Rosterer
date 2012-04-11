using System;
using System.Web.Mvc;
using Rosterer.Frontend.Models;

namespace Rosterer.Frontend.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(int? month, int? year)
        {   
            return View(new MonthModel(month ?? DateTime.Now.Month, year ?? DateTime.Now.Year));
        }

        public ActionResult About()
        {
            return View();
        }
    }
}