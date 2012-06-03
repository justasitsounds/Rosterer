using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rosterer.Domain.Entities;
using Rosterer.Frontend.Models;

namespace Rosterer.Frontend.Controllers
{
    public class WeekController : BaseController
    {
        // GET: /Week/year/week
        public ActionResult Index(int? year, int? week, int? targetMonth)
        {
            int yearnumber = year ?? DateTime.Now.Year;
            int weekNumber = week ?? 1;
            int monthNumber = targetMonth ?? DateTime.Now.Month;
            var calendarWeek = new CalendarWeek(yearnumber, weekNumber, monthNumber);
            var eventBookings = RavenSession.Query<CalendarBooking>().Where(c => c.StartTime > calendarWeek.Start && c.StartTime < calendarWeek.End).ToList();
            var mappedBookings = AutoMapper.Mapper.Map<List<CalendarBooking>, List<EventViewModel>>(eventBookings);
            foreach (var calendarDay in calendarWeek.Days)
            {
                calendarDay.LoadBookings(mappedBookings);
            }
            return PartialView(calendarWeek);
        }
    }
}
