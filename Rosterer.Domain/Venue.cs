﻿namespace Rosterer.Domain
{
    public class Venue
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Notes { get; set; }

        public Venue()
        {
            Id = "venues/";
        }
    }

    public struct Location
    {
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
    }
}