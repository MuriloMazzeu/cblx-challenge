using AutoMapper;
using CblxChallenge.Domain.Entities;
using CblxChallenge.Domain.ViewModels;

namespace CblxChallenge.Domain.Mappers
{
    public class EntityQueryMapper : Profile
    {
        public EntityQueryMapper()
        {
            CreateMap<FreighterTransportEntity, ReceivedMineralsData>()
                .ForMember(t => t.StartAt, o => o.MapFrom(e => e.StartAt.ToDateTime()))
                .ForMember(t => t.EndAt, o => o.MapFrom(e => e.EndAt.Value.ToDateTime()));
        }
    }
}
