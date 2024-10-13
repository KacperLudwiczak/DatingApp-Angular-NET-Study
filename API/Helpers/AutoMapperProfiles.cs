using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
namespace API.Helpers;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>()
            .ForMember(destination => destination.Age, option => option.MapFrom(source => source.DateOfBirth.CalculateAge()))
            .ForMember(destination => destination.PhotoUrl, option =>
                option.MapFrom(source => source.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdateDto, AppUser>();
        CreateMap<RegisterDto, AppUser>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)));
        CreateMap<Message, MessageDto>()
            .ForMember(destination => destination.SenderPhotoUrl, option => option.MapFrom(source => source.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url))
            .ForMember(destination => destination.RecipientPhotoUrl, option => option.MapFrom(source => source.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url));
    }
}