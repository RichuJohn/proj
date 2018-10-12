using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using FitnessGuru_Main.Dtos;
using FitnessGuru_Main.Models;

namespace FitnessGuru_Main.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Session, SessionDto>();
            Mapper.CreateMap<Session, SessionCreateDto>();
            Mapper.CreateMap<SessionCreateDto, Session>();
            Mapper.CreateMap<SessionEditDto, Session>();
            Mapper.CreateMap<SessionDto, Session>();
            Mapper.CreateMap<GymMember, GymMemberDto>();
            Mapper.CreateMap<Session, SessionCalendarDto>()
                .ForMember(dest => dest.Title, src => src.MapFrom(s => s.SessionName))
                .ForMember(dest => dest.Start, src => src.MapFrom(s => s.SessionAt))
                .ForMember(dest => dest.TrainerName, src =>src.MapFrom( s=> s.GymMember.FirstName));
            Mapper.CreateMap<Session, JoinedSessionDto>()
                .BeforeMap((s, d) => d.Joined = true)
                .ForMember(dest => dest.Title, src => src.MapFrom(s => s.SessionName))
                .ForMember(dest => dest.Start, src => src.MapFrom(s => s.SessionAt))
                .ForMember(dest => dest.TrainerName, src => src.MapFrom(s => s.GymMember.FirstName));

            Mapper.CreateMap<Session, UpcomingSessionDto>()
                .BeforeMap((s, d) => d.Joined = false)
                .ForMember(dest => dest.Title, src => src.MapFrom(s => s.SessionName))
                .ForMember(dest => dest.Start, src => src.MapFrom(s => s.SessionAt))
                .ForMember(dest => dest.TrainerName, src => src.MapFrom(s => s.GymMember.FirstName));

            Mapper.CreateMap<JoinedSessionDto, MemberSessionDto>();
            Mapper.CreateMap<UpcomingSessionDto, MemberSessionDto>();
        }
    }
}