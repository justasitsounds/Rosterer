using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rosterer.Domain.Events;

namespace Rosterer.Domain
{
    public class EditSession : IHandle<EventModifiedEvent>
    {
        private List<CalendarEvent> ModifiedEvents { get; set; }

        public EditSession()
        {
            ModifiedEvents = new List<CalendarEvent>();
        }

        public void Handle(EventModifiedEvent args)
        {
            if (ModifiedEvents.Any(e => args.ModifiedEvent.Id == e.Id)) return;
            ModifiedEvents.Add(args.ModifiedEvent);
        }

        public void Publish()
        {
            ModifiedEvents.ForEach(e => e.Publish());
            ModifiedEvents.Clear();
        }
    }
}
