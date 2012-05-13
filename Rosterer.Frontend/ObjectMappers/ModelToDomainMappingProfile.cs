﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using AutoMapper;
using Rosterer.Domain;
using Rosterer.Frontend.Models;

namespace Rosterer.Frontend.ObjectMappers
{
    public class ModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return GetType().Name; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<RegisterModel,User>()
                .ForMember(user => user.DisplayColour, opt => opt.ResolveUsing(r => Color.FromArgb(255,r.Red,r.Green,r.Blue)))
                .ForMember(user => user.EmailAddress, opt => opt.MapFrom(r => r.Email))
                .ForMember(user => user.FirstName, opt => opt.MapFrom(r => r.FirstName))
                .ForMember(user => user.LastName, opt => opt.MapFrom(r => r.LastName))
                ;

            Mapper.CreateMap<VenueModel,Venue>()
                .ForMember(venue => venue.Location, opt => opt.ResolveUsing(r => new Location()
                                                                                     {
                                                                                         Address1 = r.Address1,
                                                                                         Address2 = r.Address2,
                                                                                         State = Enum.GetName(typeof(State),r.State),
                                                                                         Longitude = r.Longitude,
                                                                                         Latitude = r.Latitude
                                                                                     }));

            ;
        }
    }
}