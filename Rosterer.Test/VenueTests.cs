﻿using System;
using System.Drawing;
using System.Linq;
using System.Text;
using Rosterer.Domain.Entities;
using Rosterer.Test;

namespace Rosterer.Test
{
    public class VenueTests
    {
        public void VenueBelongsToRegion()
        {
            var region = new Rosterer.Domain.Entities.Region();
            
            region.Name = "Sydney";
            
        }
    }

    
}

