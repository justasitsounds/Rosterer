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
        private readonly List<CalendarBooking> _modifiedEvents;
        private readonly HttpSessionStateBase session;

        public EditSession(HttpSessionStateBase session)
        {
            this.session = session;
            if (session["modifiedEvents"] != null)
            {
                _modifiedEvents = session["modifiedEvents"] as List<CalendarBooking>;
            }
            else
            {
                _modifiedEvents = new List<CalendarBooking>();
            }
        }

        public void Handle(BookingModifiedEvent args)
        {
            if (_modifiedEvents.Any(e => args.ModifiedBooking.Id == e.Id)) return;
            _modifiedEvents.Add(args.ModifiedBooking);
        }

        public IList<CalendarBooking> RegisteredEvents
        {
            get { return _modifiedEvents; }
        }

        public void Publish()
        {
            _modifiedEvents.ForEach(e => e.Publish());
            _modifiedEvents.Clear();
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
