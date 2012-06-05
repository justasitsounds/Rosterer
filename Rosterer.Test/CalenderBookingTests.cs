using System;
using Rosterer.Domain;
using Rosterer.Domain.Entities;
using Rosterer.Domain.Events;
using Xunit;

namespace Rosterer.Test
{
    public class CalenderBookingTests
    {
        [Fact]
        public void CanCreateCalendarBooking()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            var cEvent = new CalendarBooking(start, end);
            Assert.Equal(cEvent.StartTime, start);
            Assert.Equal(cEvent.EndTime, end);
        }

        [Fact]
        public void CalendarBookingsEndAfterTheyStart()
        {
            DateTime start = DateTime.Now.AddDays(1);
            DateTime end = start.AddHours(-1);
            Assert.Throws<ArgumentOutOfRangeException>(() => { new CalendarBooking(start, end); });
        }

        [Fact]
        public void CalendarBookingsAreInDraftStateByDefault()
        {
            var cEvent = new CalendarBooking(DateTime.Now, DateTime.Now.AddHours(2));
            Assert.Equal(cEvent.PublishState,PublishState.Draft);
        }

        [Fact]
        public void CalendarBookingsCanBePublished()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            var cEvent = new CalendarBooking(start, end);
            Assert.True(cEvent.PublishState == PublishState.Draft);
            Assert.True(cEvent.PublishDate == DateTime.MinValue);
            cEvent.Publish();
            Assert.True(cEvent.PublishState == PublishState.Published);
            Assert.True(cEvent.PublishDate != DateTime.MinValue);
        }

        [Fact]
        public void PublishingRaisesPublishEvent()
        {
            var start = DateTime.Now;
            var end = start.AddHours(1);
            var cEvent = new CalendarBooking(start, end);

            CalendarBooking publishedBooking = null;
            DomainEvents.Register<BookingPublishedEvent>(p => publishedBooking = p.PublishedBooking);
            cEvent.Publish();

            Assert.Equal(publishedBooking,cEvent);
        }

        [Fact]
        public void EditingEventRaisesModifiedEvent()
        {
            var start = DateTime.Now;
            var end = start.AddHours(1);
            var cEvent = new CalendarBooking(start, end);

            CalendarBooking modifiedBooking = null;
            DomainEvents.Register<BookingModifiedEvent>(p => modifiedBooking = p.ModifiedBooking);
            cEvent.EndTime = end.AddHours(1);

            Assert.Equal(modifiedBooking,cEvent);
        }
        
        [Fact]
        public void ChangingEventPropertiesChangesPublishStateAndModifiedDate()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            var cEvent = new CalendarBooking(start, end);
            cEvent.Publish();
            Assert.True(cEvent.PublishState == PublishState.Published);
            var lastModifiedDate = cEvent.LastModified;

            var venue = new Venue();
            cEvent.Venue = venue;
            Assert.Equal(cEvent.PublishState,PublishState.Draft);
            var updatedLastModifiedDate = cEvent.LastModified;
            //Assert.True(updatedLastModifiedDate > lastModifiedDate);
            cEvent.Publish();
            Assert.True(cEvent.PublishDate > lastModifiedDate);
            Assert.Equal(cEvent.PublishState, PublishState.Published);

            cEvent.Venue = venue;
            Assert.Equal(cEvent.PublishState,PublishState.Published);
            Assert.Equal(cEvent.LastModified, updatedLastModifiedDate);
        }
    }
}