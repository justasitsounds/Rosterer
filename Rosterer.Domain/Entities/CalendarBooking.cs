﻿using System;
using System.Collections.Generic;
using System.Linq;
using Rosterer.Domain.Events;

namespace Rosterer.Domain.Entities
{
    public class CalendarBooking
    {
        private readonly List<User> _staff = new List<User>();
        private DateTime _endTime;
        private DateTime _startTime;
        private Venue _venue;

        public CalendarBooking(DateTime start, DateTime end, string bookingOwnerId, Venue venue)
        {
            if (end < start)
                throw new ArgumentOutOfRangeException("end", "Event cannot end before it starts");
            _startTime = start;
            _endTime = end;
            PublishState = PublishState.Draft;
            LastModified = DateTime.Now;
            PublishDate = DateTime.MinValue;
            BookingOwnerId = bookingOwnerId;
            _venue = venue;
            PropertyChanged();
        }

        public string BookingOwnerId { get; private set; }

        public string Id { get; set; }

        public Venue Venue
        {
            get { return _venue; }
            set
            {
                if (_venue == value) return;
                _venue = value;
                PropertyChanged();
            }
        }

        public List<User> Staff
        {
            get { return _staff; }
        }

        public DateTime LastModified { get; private set; }
        public PublishState PublishState { get; private set; }
        public DateTime PublishDate { get; private set; }

        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime == value) return;
                _startTime = value;
                PropertyChanged();
            }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime == value) return;
                _endTime = value;
                PropertyChanged();
            }
        }

        public void AddStaff(User staffmember)
        {
            if (_staff.Contains(staffmember)) return;
            _staff.Add(staffmember);
            PropertyChanged();
        }

        public void RemoveStaff(string email)
        {
            if (_staff.All(s => s.EmailAddress != email)) return;
            _staff.RemoveAll(u => u.EmailAddress == email);
            PropertyChanged();
        }

        public void Publish()
        {
            PublishState = PublishState.Published;
            PublishDate = DateTime.Now;
            DomainEvents.Raise(new BookingPublishedEvent {PublishedBooking = this});
        }

        private void PropertyChanged()
        {
            PublishState = PublishState.Draft;
            LastModified = DateTime.Now;
            DomainEvents.Raise(new BookingModifiedEvent {ModifiedBooking = this});
        }
    }

    public enum PublishState
    {
        Draft,
        Published
    }
}