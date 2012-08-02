using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rosterer.Domain.Entities;

namespace Rosterer.Frontend.Models
{
    public class EventViewModel
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public VenueModel Venue { get; set; }
        public IEnumerable<StaffModel> Staff { get; set; }
        public PublishState PublishState { get; set; }
        
    }
}