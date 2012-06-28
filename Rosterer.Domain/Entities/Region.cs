using System.Collections.Generic;
using System.Drawing;

namespace Rosterer.Domain.Entities
{
    public class Region
    {
        public string Name { get; set; }
        public Color Color { get; set; }
    }

    public static class RegionList
    {
        public static List<Region> Regions { get
        {
            return new List<Region>()
                       {
                           new Region()
                           {                                   
                               Name = "Sydney",Color = Color.Orange
                           },
                           new Region()
                           {                                   
                               Name = "Hunter",Color = Color.Olive
                           },
                           new Region()
                           {                                   
                               Name = "NSW Other",Color = Color.DodgerBlue
                           },
                           new Region()
                           {                                   
                               Name = "Victoria",Color = Color.PaleGreen
                           },
                           new Region()
                           {                                   
                               Name = "South Australia",Color = Color.OrangeRed
                           },
                           new Region()
                           {                                   
                               Name = "West Australia",Color = Color.PaleVioletRed
                           },
                           new Region()
                           {                                   
                               Name = "Queensland",Color = Color.Maroon
                           },
                           new Region()
                           {                                   
                               Name = "New Zealand",Color = Color.Gray
                           }
                       };
        } } 
    }
    
}