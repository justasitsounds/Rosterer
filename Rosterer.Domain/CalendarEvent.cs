using System;

namespace Rosterer.Domain
{
    public class CalendarEvent
    {
        private readonly DateTime _endTime;
        private readonly DateTime _startTime;

        public CalendarEvent(DateTime start, DateTime end)
        {
            if (end < start)
                throw new ArgumentOutOfRangeException("end", "Event cannot end before it starts");
            _startTime = start;
            _endTime = end;
        }

        public string Name { get; set; }

        public DateTime StartTime
        {
            get { return _startTime; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
        }
    }
}