﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rosterer.Domain.Entities;

namespace Rosterer.Domain.Events
{
    public class BookingPublishedEvent : IDomainEvent
    {
        public CalendarBooking PublishedBooking { get; set; }
    }

    
}
