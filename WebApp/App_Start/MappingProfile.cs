using ApplicationCore.DTO;
using AutoMapper;
using Infrastructure.Aggregets.MessageAgg;
using Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<ApplicationUser, UserDTO>();
            Mapper.CreateMap<UserDTO, ApplicationUser>();
            Mapper.CreateMap<MessageDTO, Message>();
            Mapper.CreateMap<Message, MessageDTO>()
                
                .ForMember(dest=>dest.MessageSentString, opt=>opt.MapFrom(src =>src.MessageSent.ToString()));
        }
       
          
    }
}