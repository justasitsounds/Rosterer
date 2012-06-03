using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Rosterer.Domain.Entities;
using Rosterer.Domain.Events;

namespace Rosterer.Domain
{
    public class EditSession : ISessionState
    {
        private List<CalendarBooking> ModifiedEvents { get; set; }
        private readonly HttpSessionStateBase session;

        public EditSession(HttpSessionStateBase session)
        {
            this.session = session;
            ModifiedEvents = new List<CalendarBooking>();
        }

        public void Handle(BookingModifiedEvent args)
        {
            if (ModifiedEvents.Any(e => args.ModifiedBooking.Id == e.Id)) return;
            ModifiedEvents.Add(args.ModifiedBooking);
        }

        public IList<string> RegisteredEvents
        {
            get { return ModifiedEvents.Select(e => e.ToString()).ToList(); }
        }

        public void Publish()
        {
            ModifiedEvents.ForEach(e => e.Publish());
            ModifiedEvents.Clear();
        }

        public void Clear()
        {
            session.RemoveAll();
        }

        public void Delete(string key)
        {
            session.Remove(key);
        }

        public object Get(string key)
        {
            return session[key];
        }

        public void Store(string key, object value)
        {
            session[key] = value;
        }
    }

    public interface ISessionState : IHandle<BookingModifiedEvent>
    {
        void Clear();
        void Delete(string key);
        object Get(string key);
        void Store(string key, object value);
    }

    public static class SessionExtensions
    {
        public static T Get<T>(this ISessionState sessionState, string key) where T : class
        {
            return sessionState.Get(key) as T;
        }
    }
}
