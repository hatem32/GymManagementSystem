using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _MemberRepository;
        public MemberService(IGenericRepository<Member> MemberRepository)
        {
            _MemberRepository = MemberRepository;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            //check email
            var EmailExists = await _MemberRepository.AnyAsync(x => x.Email == model.Email, ct);
            //check phone
            var PhoneExists = await _MemberRepository.AnyAsync(x => x.Phone == model.Phone, ct);

            //email and phone exists return false

            if (EmailExists || PhoneExists) return false;

            //else return true and add member

            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = (int)model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                HealthRecord = new HealthRecord()
                {
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Weight = model.HealthRecordViewModel.Weight,
                    Height = model.HealthRecordViewModel.Height,
                    Note = model.HealthRecordViewModel.Note,
                }
            };

            var result = await _MemberRepository.AddAsync(member);
            return result > 0;

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _MemberRepository.GetAllAsync(ct: ct);
            if (!members.Any()) return [];

            var membersViewModels = members.Select(x => new MemberViewModel()
            {
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Photo = x.Photo,
                Gender = x.Gender.ToString(),
                Id = x.Id

            });
            return membersViewModels;
        }
    }
}
