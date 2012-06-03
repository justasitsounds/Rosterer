using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Rosterer.Domain;
using Rosterer.Domain.Entities;
using Rosterer.Frontend.Models;

namespace Rosterer.Frontend.Controllers
{
    public class EventController : BaseController
    {
        public ISessionState CurrentEditSession { get; set; }

        public ActionResult Index()
        {
            var bookings = RavenSession.Query<CalendarBooking>().Take(10).ToList();
            return PartialView("EventView", AutoMapper.Mapper.Map<IList<CalendarBooking>, IList<EventViewModel>>(bookings));            
        }

        // GET: /Event/List?start=date&end=date
        [HttpGet]
        public JsonResult List(DateTime start, DateTime end)
        {
            var bookings = RavenSession.Query<CalendarBooking>().Where(c => c.StartTime.Date >= start.Date && c.StartTime.Date < end.Date).ToList();
            //return PartialView("EventView", AutoMapper.Mapper.Map<IList<CalendarBooking>, IList<EventViewModel>>(bookings));
            return Json(AutoMapper.Mapper.Map<IList<CalendarBooking>, IList<EventViewModel>>(bookings), JsonRequestBehavior.AllowGet);
        }
        

        // GET: /Event/Create
        public ActionResult Create(DateTime startDate)
        {
            var eventFormModel = new EventFormModel();
            eventFormModel.StartDate = eventFormModel.EndDate = startDate;
            PopulateEventDropDowns(eventFormModel);

            return PartialView("EventForm", eventFormModel);
        }

        private void PopulateEventDropDowns(EventFormModel eventFormModel)
        {
            eventFormModel.SetStaffList(
                AutoMapper.Mapper.Map<IEnumerable<User>, IEnumerable<RosterMembershipUser>>(
                    RavenSession.Query<User>().ToList()));
            eventFormModel.SetVenuesList(
                AutoMapper.Mapper.Map<IEnumerable<Venue>, IEnumerable<VenueModel>>(
                    RavenSession.Query<Venue>().ToList()));
        }

        // GET: /Event/Details/5
        public ActionResult Details(string id)
        {
            var eventModel = RavenSession.Load<CalendarBooking>(id);
            var eventView = AutoMapper.Mapper.Map<CalendarBooking, EventViewModel>(eventModel);
            return PartialView("EventView", new List<EventViewModel>() { eventView });
        }

        // POST: /Event/Create
        [HttpPost]
        public ActionResult Create(EventFormModel eventFormModel)
        {
            PopulateEventDropDowns(eventFormModel);
            
            if (ModelState.IsValid)
            {
                var newEvent = AutoMapper.Mapper.Map<EventFormModel, CalendarBooking>(eventFormModel);
                newEvent.Venue = RavenSession.Load<Venue>(eventFormModel.VenueId);
                foreach (var staffmember in eventFormModel.SelectedStaff)
                {
                    var user = RavenSession.Load<User>(staffmember);
                    newEvent.AddStaff(user);
                }
                RavenSession.Store(newEvent);
                RavenSession.SaveChanges();   
            }
            ModelState.Clear();

            if (Request.IsAjaxRequest())
                return PartialView("EventForm", eventFormModel);
            else
                return RedirectToAction("Index","Home");
        } 

        
        // GET: /Event/Edit/5
        public ActionResult Edit(int id)
        {
            return RedirectToAction("Create");
        }

        
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
