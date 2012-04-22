using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rosterer.Frontend.ObjectMappers
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(i => 
            {
                i.AddProfile<DomainToModelMappingProfile>();
                i.AddProfile<ModelToDomainMappingProfile>();
            }

    );
        }
    }
}