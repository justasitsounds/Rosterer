using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Rosterer.Domain.Entities;

namespace Rosterer.Frontend.Models
{
    public class VenueModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public Region SelectedRegion { get; set; }
        public IEnumerable<SelectListItem> RegionList { get; private set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Notes { get; set; }

        public VenueModel()
        {
            RegionList = Enum.GetNames(typeof(Region))
                .Select(r => new SelectListItem() { Text = r.Replace('_',' '), Value = r, Selected = r == SelectedRegion.ToString() });
        }
    }

}