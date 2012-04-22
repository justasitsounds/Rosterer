using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Rosterer.Domain;
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
            AutoMapper.Mapper.CreateMap<User,RosterMembershipUser>()
                .ForMember(rosterUser => rosterUser.Color, opt => opt.MapFrom(user => user.DisplayColour))
                .ForMember(rosterUser => rosterUser.Email, opt => opt.MapFrom(user => user.EmailAddress))
                .ForMember(rosterUser => rosterUser.UserName, opt => opt.MapFrom(user => user.EmailAddress))
                ;
        }
    }
}