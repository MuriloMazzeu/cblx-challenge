using AutoMapper;
using CblxChallenge.Domain.Entities;
using CblxChallenge.Domain.ViewModels;
using Google.Cloud.Firestore;

namespace CblxChallenge.Domain.Mappers
{
    public class CommandEntityMapper : Profile
    {
        public CommandEntityMapper()
        {
            CreateMap<FreighterCheckinCommand, FreighterTransportEntity>()
                .ForMember(d => d.EndAt, o => o.MapFrom(e => Timestamp.FromDateTime(e.EndAt.ToUniversalTime())));

            CreateMap<FreighterCheckoutCommand, FreighterTransportEntity>()
                .ForMember(d => d.Period, o => o.MapFrom(e => e.StartAt.ToString("yyyy-MM")))
                .ForMember(d => d.StartAt, o => o.MapFrom(e => Timestamp.FromDateTime(e.StartAt.ToUniversalTime())));
        }
    }
}
