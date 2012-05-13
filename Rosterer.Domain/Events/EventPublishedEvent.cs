using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rosterer.Domain.Events
{
    public class EventPublishedEvent : IDomainEvent
    {
        public CalendarEvent PublishedEvent { get; set; }
    }
}
