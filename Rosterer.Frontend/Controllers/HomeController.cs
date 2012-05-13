using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Rosterer.Domain;
using Rosterer.Frontend.Models;

namespace Rosterer.Frontend.Controllers
{
    public class HomeController : BaseController
    {
        [Authorize]
        public ActionResult Index(int? month, int? year)
        {   
            //Logger.Debug("hey");
            return View(new MonthModel(month ?? DateTime.Now.Month, year ?? DateTime.Now.Year));
        }

        public ActionResult Help()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Venue(VenueModel venueModel)
        {
            if (ModelState.IsValid)
            {
                var venue = AutoMapper.Mapper.Map<VenueModel, Venue>(venueModel);
                RavenSession.Store(venue);
                RavenSession.SaveChanges();
            }
            ModelState.Clear();
    
            if (Request.IsAjaxRequest())
              return PartialView("VenueForm", venueModel);
            return RedirectToAction("Index");
        }

        

    }
    
}