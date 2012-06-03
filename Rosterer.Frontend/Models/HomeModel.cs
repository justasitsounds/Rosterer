using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rosterer.Frontend.Models
{
    public class HomeModel
    {
        public IList<StaffModel> Staff { get; private set; }
        public IList<VenueModel> Venues { get; private set; }

        public HomeModel(IList<StaffModel> staff, IList<VenueModel> venues)
        {
            Staff = staff;
            Venues = venues;
        }
    }
}