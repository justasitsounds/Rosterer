using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rosterer.Domain;

namespace Rosterer.Frontend.Models
{
    public class EventModel
    {
        public string Id { get; set; }

        [Display(Name = "Venue")]
        public VenueModel SelectedVenueId { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Display(Name = "Staff")]
        public List<RosterMembershipUser> SelectedStaff { get; set; }
        public PublishState PublishState { get; set; }
        public IEnumerable<SelectListItem> VenuesList { get; set; }
        public MultiSelectList StaffList { get; set; } 

        public void SetVenuesList(IEnumerable<VenueModel> venues)
        {
            VenuesList = venues.Select(v => new SelectListItem() {Text = v.Name, Value = v.Id});
        }

        public void SetStaffList(IEnumerable<RosterMembershipUser> staff)
        {
            StaffList = new MultiSelectList(staff, "Email", "Email", SelectedStaff);
        }
    }

    
}