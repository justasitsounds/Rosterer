using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Rosterer.Domain;
using Rosterer.Domain.Entities;
using Rosterer.Frontend.Models;

namespace Rosterer.Frontend.ObjectMappers
{
    public class DomainToModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return GetType().Name; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<User,RosterMembershipUser>()
                .ForMember(rosterUser => rosterUser.Color, opt => opt.MapFrom(user => user.DisplayColour))
                .ForMember(rosterUser => rosterUser.Email, opt => opt.MapFrom(user => user.EmailAddress))
                .ForMember(rosterUser => rosterUser.UserName, opt => opt.MapFrom(user => user.EmailAddress))
                ;
            Mapper.CreateMap<Venue, VenueModel>()
                .ForMember(model => model.Address1, opt => opt.MapFrom(venue => venue.Location.Address1))
                .ForMember(model => model.Address2, opt => opt.MapFrom(venue => venue.Location.Address2))
                .ForMember(model => model.State, opt => opt.MapFrom(venue => (State)Enum.Parse(typeof(State),venue.Location.State)))
                .ForMember(model => model.Latitude, opt => opt.MapFrom(venue => venue.Location.Latitude))
                .ForMember(model => model.Longitude, opt => opt.MapFrom(venue => venue.Location.Longitude))
                ;
            Mapper.CreateMap<User, StaffModel>()
                .ForMember(staff => staff.Name,
                           opt => opt.MapFrom(user => string.Format("{0} {1}", user.FirstName, user.LastName)))
                .ForMember(staff => staff.Initials, opt => opt.MapFrom(user => string.Format("{0}{1}", user.FirstName.Substring(0,1), user.LastName.Substring(0,1))))
                .ForMember(staff => staff.Color, opt => opt.MapFrom(user => user.DisplayColour));
            Mapper.CreateMap<CalendarBooking, EventViewModel>()
                .ForMember(eventview => eventview.End, opt => opt.MapFrom(booking => booking.EndTime))
                .ForMember(eventview => eventview.Start, opt => opt.MapFrom(booking => booking.StartTime))
                ;
        }
    }
}