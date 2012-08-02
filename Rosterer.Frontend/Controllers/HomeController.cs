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
            List<User> userRecords = RavenSession.Query<User>().ToList();
            var staff = AutoMapper.Mapper.Map<List<User>,List<StaffModel>>(userRecords);
            List<Venue> venuerecords = RavenSession.Query<Venue>().ToList();
            var venue = AutoMapper.Mapper.Map<List<Venue>,List<VenueModel>>(venuerecords);
            return View(new HomeModel(staff,venue));
        }

        public ActionResult Help()
        {
            return View();
        }
    }
}