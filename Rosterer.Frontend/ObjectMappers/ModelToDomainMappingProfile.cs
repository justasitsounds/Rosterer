using System;
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
        }
    }
}