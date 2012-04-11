using System;
using System.Collections.Generic;
using System.Globalization;
using Rosterer.Domain;

namespace Rosterer.Frontend.Models
{
    public class MonthModel
    {
        public MonthModel(int month, int year)
        {
            DateTimeFormatInfo dtf = CultureInfo.CurrentCulture.DateTimeFormat;
            Year = year;
            MonthNumber = month;
            MonthName = dtf.GetMonthName(month);
            CalendarWeeks = new List<CalendarWeek>();

            var firstofMonth = new DateTime(year, month, 1);
            PreviousMonth = firstofMonth.AddMonths(-1);
            NextMonth = firstofMonth.AddMonths(1);

            CalendarWeeks.Add(new CalendarWeek(firstofMonth));
            CalendarWeeks.Add(new CalendarWeek(firstofMonth.AddDays(7)));
            CalendarWeeks.Add(new CalendarWeek(firstofMonth.AddDays(14)));
            CalendarWeeks.Add(new CalendarWeek(firstofMonth.AddDays(21)));
            CalendarWeeks.Add(new CalendarWeek(firstofMonth.AddDays(28)));
        }

        public DateTime PreviousMonth { get; private set; }
        public DateTime NextMonth { get; private set; }

        public int Year { get; set; }
        public string MonthName { get; set; }
        public int MonthNumber { get; private set; }
        public List<CalendarWeek> CalendarWeeks { get; set; }
    }
}