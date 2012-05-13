using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public string Notes { get; set; }
    }


    public enum State
    {
        NSW,VIC,SA,TAS,WA,NT,QLD
    }
}