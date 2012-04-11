using System;
using Rosterer.Domain;
using Xunit;

namespace Rosterer.Test
{
    public class CalenderEventTests
    {
        [Fact]
        public void CanCreateCalendarEvent()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            var cEvent = new CalendarEvent(start, end)
                             {
                                 Name = "My Event"
                             };
            Assert.Equal(cEvent.StartTime, start);
            Assert.Equal(cEvent.EndTime, end);
        }

        [Fact]
        public void CalendarEventsEndAfterTheyStart()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(-1);
            Assert.Throws<ArgumentOutOfRangeException>(() => { new CalendarEvent(start, end); });
        }
    }
}