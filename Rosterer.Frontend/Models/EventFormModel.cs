using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rosterer.Domain;
using Rosterer.Domain.Entities;
using Rosterer.Frontend.Plumbing;

namespace Rosterer.Frontend.Models
{
    public class EventFormModel
    {
        public string Id { get; set; }

        [Display(Name = "Venue")]
        public string VenueId { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [CompareDatesAttribute("StartDate",ErrorMessage = "EndDate must be greater than Startdate")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Staff")]
        public IEnumerable<string> SelectedStaff { get; set; }
        public PublishState PublishState { get; set; }
        public IEnumerable<SelectListItem> VenuesList { get; set; }
        public MultiSelectList StaffList { get; set; } 

        public void SetVenuesList(IEnumerable<VenueModel> venues)
        {
            VenuesList = venues.Select(v => new SelectListItem() {Text = v.Name, Value = v.Id, Selected = v.Id == VenueId});
            
        }

        public void SetStaffList(IEnumerable<RosterMembershipUser> staff)
        {
            StaffList = new MultiSelectList(staff, "Id", "Email", SelectedStaff);
        }
    }

    
}