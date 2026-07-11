using AutoMapper;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            MapMember();
            MapSession();

        }

        private void MapMember()
        {
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));


            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();


            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));


            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    dest.UpdatedAt = DateTime.Now;
                });


            CreateMap<CreateMemberViewModel, Member>()
                  .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                  {
                      BuildingNumber = src.BuildingNumber,
                      City = src.City,
                      Street = src.Street
                  }))
                  .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));
        }

        private void MapSession()
        {

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Session, SessionViewModel>()
                 .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                 .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name));

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }
    }
}
