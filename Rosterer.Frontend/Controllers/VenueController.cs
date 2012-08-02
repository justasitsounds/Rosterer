using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Rosterer.Domain.Entities;
using Rosterer.Frontend.Models;

namespace Rosterer.Frontend.Controllers
{
    public class VenueController : BaseController
    {

        //
        // GET: /Venue/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Venue/Create

        public ActionResult Create()
        {
            var model = new VenueModel();
            
            
            return PartialView(model);
        } 

        //
        // POST: /Venue/Create

        [HttpPost]
        public ActionResult Create(VenueModel venueModel)
        {

            if (ModelState.IsValid)
            {
                var venue = AutoMapper.Mapper.Map<VenueModel, Venue>(venueModel);
                venue.Region = venueModel.SelectedRegion;
                RavenSession.Store(venue);
                RavenSession.SaveChanges();
            }
            ModelState.Clear();
            return PartialView("Create", venueModel);
            
        }
        
        //
        // GET: /Venue/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Venue/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return null;
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Venue/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Venue/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return null;
            }
            catch
            {
                return View();
            }
        }

        public ActionResult VenueNav()
        {
            var venues = RavenSession.Query<Venue>().ToList();
            return View(Mapper.Map<IList<Venue>, IList<VenueModel>>(venues));
        }
    }
}
