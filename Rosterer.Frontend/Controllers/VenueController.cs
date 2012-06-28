using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rosterer.Domain.Entities;
using Rosterer.Frontend.Models;

namespace Rosterer.Frontend.Controllers
{
    public class VenueController : BaseController
    {
        //
        // GET: /Venue/

        public ActionResult Index()
        {
            return View();
        }

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
            IEnumerable<RegionModel> regionModels = AutoMapper.Mapper.Map<IEnumerable<Region>, IEnumerable<RegionModel>>(RegionList.Regions);
            model.SetRegionList(regionModels);
            
            return PartialView(model);
        } 

        //
        // POST: /Venue/Create

        [HttpPost]
        public ActionResult Create(VenueModel venueModel)
        {
            IEnumerable<RegionModel> regionModels = AutoMapper.Mapper.Map<IEnumerable<Region>, IEnumerable<RegionModel>>(RegionList.Regions);
            venueModel.SetRegionList(regionModels);
            if (ModelState.IsValid)
            {
                var venue = AutoMapper.Mapper.Map<VenueModel, Venue>(venueModel);
                venue.Region = RegionList.Regions.Single(r => r.Name == venueModel.RegionName);
                RavenSession.Store(venue);                
            }
            ModelState.Clear();

            if (Request.IsAjaxRequest())
                return PartialView("Create", venueModel);
            return RedirectToAction("Index");
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
 
                return RedirectToAction("Index");
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
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
