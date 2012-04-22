namespace Rosterer.Domain
{
    public class Venue
    {
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Notes { get; set; }
    }

    public struct Location
    {
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Address { get; set; }
    }
}