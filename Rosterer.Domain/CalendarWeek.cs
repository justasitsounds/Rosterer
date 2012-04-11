using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rosterer.Domain
{
    public class CalendarWeek
    {
        private DateTime _end;
        private DateTime _start;

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