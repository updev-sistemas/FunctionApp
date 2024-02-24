using AutoMapper;
using FunctionApp.Domains.Bson;
using FunctionApp.Domains.ValueObject;

namespace FunctionApp.Infrastructure.Profiles;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        _ = CreateMap<PersonBson, PersonValueObject>()
               .ForCtorParam(nameof(PersonValueObject.Id), opt => opt.MapFrom(src => src.Id))
               .ForCtorParam(nameof(PersonValueObject.Name), opt => opt.MapFrom(src => src.Name))
               .ForCtorParam(nameof(PersonValueObject.Email), opt => opt.MapFrom(src => src.Email))
               .ForCtorParam(nameof(PersonValueObject.Birthday), opt => opt.MapFrom(src => src.Birthday))
               .ForCtorParam(nameof(PersonValueObject.CreatedAt), opt => opt.MapFrom(src => src.CreatedAt))
               .ForCtorParam(nameof(PersonValueObject.UpdatedAt), opt => opt.MapFrom(src => src.UpdatedAt));

        _ = CreateMap<PersonValueObject, PersonBson>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday))
              .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt ?? DateTime.UtcNow))
              .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? DateTime.UtcNow));

    }
}
