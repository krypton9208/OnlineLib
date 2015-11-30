using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using OnlineLib.App.Models;
using OnlineLib.Models;

namespace OnlineLib.App.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<RegisterViewModel, LibUser>();
        }
    }
}
