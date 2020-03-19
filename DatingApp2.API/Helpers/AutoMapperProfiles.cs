using AutoMapper;
using DatingApp2.API.Models;
using DatingApp2.API.DTO;
using System.Linq;

namespace DatingApp2.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()

        {
            CreateMap<Users, UserForListDTO>()
                .ForMember(dest => dest.PhotoURL,
                                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Users, UserDetailsDTO>()
                .ForMember(dest => dest.PhotoURL,
                                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<UserForEditDTO, Users>();
            CreateMap<Photo, PhotosDetailsDTO>();
            CreateMap<PhotoForCreationDTO, Photo>();
            CreateMap<Photo, PhotoForReturnDTO>();
            CreateMap<UserForRegisterDTO, Users>();
            CreateMap<MessageForCreationDTO, Message>().ReverseMap();            
            CreateMap<Message, MessageToReturnDTO>()
                .ForMember(dest => dest.SenderPhotoURL, 
                                opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoURL,
                                opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
        }
    }
}