using System;

namespace Rosterer.Domain
{
    public class CalendarDay
    {
        private DateTime _date;

        public CalendarDay(DateTime date)
        {
            _date = date;
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
    }
}