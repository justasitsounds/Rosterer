using System;

namespace Rosterer.Domain.Entities
{
    public class LogEntry
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}