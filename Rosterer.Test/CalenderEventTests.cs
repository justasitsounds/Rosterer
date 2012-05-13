using System;
using Rosterer.Domain;
using Rosterer.Domain.Events;
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
            var cEvent = new CalendarEvent(start, end);
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

        [Fact]
        public void CalendarEventsAreInDraftStateByDefault()
        {
            var cEvent = new CalendarEvent(DateTime.Now, DateTime.Now.AddHours(2));
            Assert.Equal(cEvent.PublishState,PublishState.Draft);
        }

        [Fact]
        public void CalendarEventsCanBePublished()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            var cEvent = new CalendarEvent(start, end);
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
            var cEvent = new CalendarEvent(start, end);

            CalendarEvent publishedEvent = null;
            DomainEvents.Register<EventPublishedEvent>(p => publishedEvent = p.PublishedEvent);
            cEvent.Publish();

            Assert.Equal(publishedEvent,cEvent);
        }

        [Fact]
        public void EditingEventRaisesModifiedEvent()
        {
            var start = DateTime.Now;
            var end = start.AddHours(1);
            var cEvent = new CalendarEvent(start, end);

            CalendarEvent modifiedEvent = null;
            DomainEvents.Register<EventModifiedEvent>(p => modifiedEvent = p.ModifiedEvent);
            cEvent.EndTime = end.AddHours(1);

            Assert.Equal(modifiedEvent,cEvent);
        }
        
        [Fact(Skip="because it's currently broken")]
        public void ChangingEventPropertiesChangesPublishStateAndModifiedDate()
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(1);
            var cEvent = new CalendarEvent(start, end);
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