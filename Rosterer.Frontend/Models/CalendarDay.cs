using System;
using System.Collections.Generic;
using System.Linq;
using Rosterer.Domain.Entities;

namespace Rosterer.Frontend.Models
{
    public class CalendarDay
    {
        private DateTime _date;

        public CalendarDay(DateTime date)
        {
            _date = date;
            Bookings = new List<EventViewModel>();
        }

        public string Name
        {
            get { return _date.ToString("dddd"); }
        }

        public DateTime Date
        {
            get { return _date.Date; }
        }

        public bool IsToday()
        {
            return _date.Date == DateTime.Today;
        }

        public bool IsInCurrentMonth(int currentmonth)
        {
            return currentmonth == _date.Month;
        }

        public List<EventViewModel> Bookings { get; set; }

        public void LoadBookings(List<EventViewModel> candidateBookings)
        {
            var bookings = candidateBookings.Where(b => b.Start.Date == _date.Date);
            foreach (var bookingModel in bookings)
            {
                Bookings.Add(bookingModel);
            }
        }
        
    }
}