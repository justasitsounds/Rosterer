using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Rosterer.Domain.Extensions
{
    public static class DateTimeExtension
    {
        private static Calendar cal = CultureInfo.InvariantCulture.Calendar;

        private static DateTime IsoDate(DateTime date)
        {
            DayOfWeek day = cal.GetDayOfWeek(date);
            return date.AddDays(4 - ((int)day == 0 ? 7 : (int)day));
        }

        public static int GetIso8601WeekOfYear(this DateTime date)
        {
            date = IsoDate(date);
            return cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        
        public static int GetIso8601YearOfYear(this DateTime date)
        {
            date = IsoDate(date);
            var year = cal.GetYear(date);
            var weekNumber = cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            if ((weekNumber >= 52) && (date.Month < 12)) { year--; }
            return year;
        }

        
    }
}
