using System.Collections.Generic;
using Rosterer.Domain.Entities;

namespace Rosterer.Domain.Events
{
    public interface IHandle<T> where T : IDomainEvent
    {
        void Handle(T args);
        IList<CalendarBooking> RegisteredEvents { get; }
    }
}