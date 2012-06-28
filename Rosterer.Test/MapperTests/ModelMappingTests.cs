using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Rosterer.Domain;
using Rosterer.Domain.Entities;
using Rosterer.Frontend.Models;
using Rosterer.Frontend.ObjectMappers;
using Xunit;

namespace Rosterer.Test.MapperTests
{
    public class ModelMappingTests
    {
        public ModelMappingTests()
        {
            AutoMapperConfiguration.Configure();
        }

        [Fact]
        public void DomainToModelConfigurationIsValid()
        {
            var profileInstance = new DomainToModelMappingProfile();
            Mapper.AssertConfigurationIsValid(profileInstance.ProfileName);
        }

        [Fact]
        public void ModelToDomainConfigurationIsValid()
        {
            var profileInstance = new ModelToDomainMappingProfile();
            Mapper.AssertConfigurationIsValid(profileInstance.ProfileName);

            var registermodel = new RegisterModel
                                    {
                                        FirstName = "james",
                                        LastName = "prendergast",
                                        Email = "james.prendergast@gmail.com",
                                        Color="#ffffff"
                                    };

            var user = Mapper.Map<RegisterModel, User>(registermodel);
            Assert.Equal(registermodel.FirstName,user.FirstName);
            Assert.Equal(registermodel.LastName,user.LastName);
            Assert.Equal(registermodel.Email,user.EmailAddress);
            Assert.Equal(user.DisplayColour.R,(byte)255);
            Assert.Equal(user.DisplayColour.G,(byte)255);
            Assert.Equal(user.DisplayColour.B,(byte)255);

            var venuemodel = new VenueModel() {Address1 = "somehwre"};

            var venue = Mapper.Map<VenueModel, Venue>(venuemodel);
            Assert.Equal(venue.Location.Address1, venuemodel.Address1);

        }

    }
}
