using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Rosterer.Domain;
using Rosterer.Domain.Entities;
using Rosterer.Domain.Extensions;
using Rosterer.Frontend.Models;

namespace Rosterer.Frontend.Controllers
{
    public class EventController : BaseController
    {
        // GET: /Event/List?start=date&end=date
        [HttpGet]
        public ActionResult List(DateTime start, DateTime end)
        {
            var bookings = RavenSession.Query<CalendarBooking>().Where(c => c.StartTime.Date >= start.Date && c.StartTime.Date < end.Date).ToList();
            return PartialView("List", AutoMapper.Mapper.Map<IList<CalendarBooking>, IList<EventViewModel>>(bookings));
        }
        

        // GET: /Event/Create
        public ActionResult Create(DateTime startDate)
        {
            var eventFormModel = new EventFormModel();
            eventFormModel.StartDate = eventFormModel.EndDate = startDate;
            PopulateEventDropDowns(eventFormModel);
            
            return PartialView("Create", eventFormModel);
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
        public ActionResult Index(string id)
        {
            CalendarBooking booking;
            if (string.IsNullOrEmpty(id))
                booking = RavenSession.Query<CalendarBooking>().SingleOrDefault();
            else
                booking = RavenSession.Load<CalendarBooking>(id);
            var eventView = AutoMapper.Mapper.Map<CalendarBooking, EventViewModel>(booking);
            return PartialView("Index",  eventView);
        }

        // POST: /Event/Create
        [HttpPost]
        public ActionResult Create(EventFormModel eventFormModel)
        {
            PopulateEventDropDowns(eventFormModel);
            
            if (ModelState.IsValid)
            {
                var rosterMembershipUser = (RosterMembershipUser) Membership.GetUser();
                var domainUser =
                    AutoMapper.Mapper.Map<RosterMembershipUser, User>(rosterMembershipUser);
                var venue = RavenSession.Load<Venue>(eventFormModel.VenueId);
                var newEvent = new CalendarBooking(eventFormModel.StartDate, eventFormModel.EndDate, domainUser.Id,venue);

                foreach (var staffmember in eventFormModel.SelectedStaff)
                {
                    var user = RavenSession.Load<User>(staffmember);
                    newEvent.AddStaff(user);
                }
                
                RavenSession.Store(newEvent);
                RavenSession.SaveChanges();
                var regevents = CurrentEditSession.RegisteredEvents;
            }
            ModelState.Clear();
            var day = new DateTime(eventFormModel.StartDate.Year, eventFormModel.StartDate.Month,
                                   eventFormModel.StartDate.Day);

            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                    return Json(new { 
                        FormattedDate = eventFormModel.StartDate.YmdFormat(), 
                        StartDate = day.ToString(CultureInfo.InvariantCulture),
                        EndDate = day.AddDays(1).ToString(CultureInfo.InvariantCulture)
                    });
                return PartialView("Create", eventFormModel);
            }
            return RedirectToAction("Index", "Home");
        } 

        
        // GET: /Event/Edit/5
        public ActionResult Edit(string id)
        {
            var model = AutoMapper.Mapper.Map<CalendarBooking, EventFormModel>(RavenSession.Load<CalendarBooking>(id));
            PopulateEventDropDowns(model);
            return PartialView("Create", model);
        }

        
        // POST: /Event/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
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
        [HttpPost]
        [Authorize]
        public JsonResult Delete(string id)
        {
            var booking = RavenSession.Load<CalendarBooking>(id);
            RavenSession.Delete(booking);
            RavenSession.SaveChanges();
            return Json(new { Success=true });
        }

        //
        // POST: /Event/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}
    }
}
