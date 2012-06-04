using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rosterer.Frontend.Models
{
    public class EventViewModel
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public VenueModel Venue { get; set; }
        public IEnumerable<StaffModel> Staff { get; set; }
    }
}