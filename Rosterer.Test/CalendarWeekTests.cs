using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rosterer.Domain.Extensions;
using Rosterer.Frontend.Models;
using Xunit;

namespace Rosterer.Test
{
    public class CalendarWeekTests
    {
        private Dictionary<DateTime, Tuple<int,int>> TestDates()
        {
            return new Dictionary<DateTime, Tuple<int, int>>
                       {
                           {new DateTime(2000, 12, 31),new Tuple<int, int>(2000,52)},
                           {new DateTime(2001, 1, 1),new Tuple<int, int>(2001,1)},
                           {new DateTime(2005, 1, 1),new Tuple<int, int>(2004,53)},
                           {new DateTime(2007, 12, 31),new Tuple<int, int>(2008,1)},
                           {new DateTime(2007, 12, 30),new Tuple<int, int>(2007,52)},
                           {new DateTime(2005, 1, 2),new Tuple<int, int>(2004,53)},
                           {new DateTime(2008, 12, 28),new Tuple<int, int>(2008,52)}
                       };
        }

        [Fact]
        public void Iso8601DateTimeExtension()
        {
            var dates = TestDates();
            foreach (var dateTime in dates)
            {
                Assert.Equal(dateTime.Value.Item2, dateTime.Key.GetIso8601WeekOfYear());
                Assert.Equal(dateTime.Value.Item1, dateTime.Key.GetIso8601YearOfYear());
            }
        }

        [Fact]
        public void ReturnCorrectdaysForIsoWeek()
        {
            var dates = TestDates();
            foreach (var dateTime in dates)
            {
                var isoWeek = new CalendarWeek(dateTime.Value.Item1, dateTime.Value.Item2, dateTime.Key.Month);
                var daysOfWeek = isoWeek.Days;
                var targDay = dateTime.Key;
                Assert.Contains(targDay, daysOfWeek.Select(d => d.Date));
                //Assert.Equal(dateTime.Value.Item2, dateTime.Key.GetIso8601WeekOfYear());
                //Assert.Equal(dateTime.Value.Item1, dateTime.Key.GetIso8601YearOfYear());
            }
        }

    }

    
}
