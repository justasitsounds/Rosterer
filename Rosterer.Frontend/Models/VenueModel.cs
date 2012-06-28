using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

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

        public State State { get; set; }

        public string RegionName { get; set; }
        public IEnumerable<SelectListItem> RegionList { get; set; }

        public void SetRegionList(IEnumerable<RegionModel> regions)
        {
            RegionList =
                regions.Select(r => new SelectListItem() { Text = r.Name, Value = r.Name, Selected = !string.IsNullOrEmpty(RegionName) && r.Name == RegionName });
        }

        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public string Notes { get; set; }
        

    }


    public enum State
    {
        NSW,VIC,SA,TAS,WA,NT,QLD
    }
}