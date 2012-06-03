using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rosterer.Frontend.Models
{
    public class CalendarWeek
    {
        private DateTime _end;
        private DateTime _start;

        public DateTime Start
        {
            get { return _start; }
        }

        public DateTime End
        {
            get { return _end; }
        }

        public CalendarWeek(int year, int week, int month)
        {
            Days = new List<CalendarDay>();
            var _date = new DateTime(year, 1, 4);
            //Sunday = DayOfWeek 0
            //Monday = DayOfWeek 1
            
            var offset = 1 - (int) _date.DayOfWeek;
            if (offset > 0)
                offset -= 7;
            var firstdayofyear = _date.AddDays(offset);
            int daysToAdd = (7*(week - 1));
            var start = firstdayofyear.AddDays(daysToAdd);
            var end = start.AddDays(7);
            Monthnumber = month;
            BuildWeek(start,end);
        }

        public CalendarWeek(DateTime dayinweek)
        {
            Monthnumber = dayinweek.Month;
            DateTimeFormatInfo dtf = CultureInfo.CurrentCulture.DateTimeFormat;
            int mondayOffset = (int) dtf.FirstDayOfWeek - (int) dayinweek.DayOfWeek;
            DateTime monday = dayinweek.AddDays(mondayOffset);
            BuildWeek(monday, monday.AddDays(7));
        }

        public CalendarWeek(DateTime start, DateTime end)
        {
            BuildWeek(start, end);
        }

        public List<CalendarDay> Days { get; private set; }
        public int Monthnumber { get; private set; }

        public int WeekNumber { get; set; }

        private void BuildWeek(DateTime start, DateTime end)
        {
            _start = start;
            _end = end;
            Days = new List<CalendarDay>();
            TimeSpan diff = _end - start;
            for (int i = 0; i < diff.Days; i++)
            {
                Days.Add(new CalendarDay(_start.AddDays(i)));
            }
        }
    }
}