using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Rosterer.Domain;
using Rosterer.Domain.Entities;
using Rosterer.Frontend.Models;

namespace Rosterer.Frontend.Controllers
{
    public class HomeController : BaseController
    {
        
        [Authorize]
        public ActionResult Index(int? month, int? year)
        {
            var staff = AutoMapper.Mapper.Map<List<User>,List<StaffModel>>(RavenSession.Query<User>().ToList());
            var venue = AutoMapper.Mapper.Map<List<Venue>,List<VenueModel>>(RavenSession.Query<Venue>().ToList());
            return View(new HomeModel(staff,venue));
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