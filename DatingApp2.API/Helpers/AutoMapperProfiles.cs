using AutoMapper;
using DatingApp2.API.Models;
using DatingApp2.API.DTO;
using System.Linq;

namespace DatingApp2.API.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Users, UserForListDTO>()
                .ForMember(dest => dest.PhotoURL, 
                                opt => opt.MapFrom(src => src.Photos.FirstOrDefault().Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Users, UserDetailsDTO>()
                .ForMember(dest => dest.PhotoURL, 
                                opt => opt.MapFrom(src => src.Photos.FirstOrDefault().Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotosDetailsDTO>();
        }
    }
}