using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rosterer.Domain;
using Rosterer.Frontend.Models;

namespace Rosterer.Frontend.Controllers
{
    public class EventController : BaseController
    {
        //
        // GET: /Event/
        public ActionResult Index(EventModel eventModel)
        {
            //var eventModel = new EventModel();
            eventModel.SetStaffList(
                AutoMapper.Mapper.Map<IEnumerable<User>, IEnumerable<RosterMembershipUser>>(
                    RavenSession.Query<User>().ToList()));
            eventModel.SetVenuesList(
                AutoMapper.Mapper.Map<IEnumerable<Venue>, IEnumerable<VenueModel>>(
                    RavenSession.Query<Venue>().ToList()));
            return PartialView("EventForm", eventModel);
        }

        //
        // GET: /Event/Details/5
        public ActionResult Details(int id)
        {
            return RedirectToAction("Index");
        }

        //
        // GET: /Event/Create
        [HttpPost]
        public ActionResult Create(EventModel eventModel)
        {
            if (ModelState.IsValid)
            {
                //Save object
            }
            ModelState.Clear();

            if (Request.IsAjaxRequest())
                return PartialView("EventForm", eventModel);
            else
                return RedirectToAction("Index","Home");
        } 

        //
        // POST: /Event/Create

        
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        
        //
        // GET: /Event/Edit/5
 
        public ActionResult Edit(int id)
        {
            return RedirectToAction("Index");
        }

        //
        // POST: /Event/Edit/5

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
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Event/Delete/5
 
        public ActionResult Delete(int id)
        {
            return RedirectToAction("Index");
        }

        //
        // POST: /Event/Delete/5

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
                return RedirectToAction("Index");
            }
        }
    }
}
