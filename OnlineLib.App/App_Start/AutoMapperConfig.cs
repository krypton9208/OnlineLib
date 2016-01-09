using AutoMapper;
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
