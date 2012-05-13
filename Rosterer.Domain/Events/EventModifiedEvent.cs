using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rosterer.Domain.Events
{
    public class EventModifiedEvent : IDomainEvent
    {
        public CalendarEvent ModifiedEvent { get; set; }
    }
}
