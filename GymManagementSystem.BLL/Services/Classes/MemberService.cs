using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService( IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            //check email
            var EmailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email, ct);
            //check phone
            var PhoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone, ct);

            //email and phone exists return false

            if (EmailExists || PhoneExists) return false;

            //else return true and add member

            var member = _mapper.Map<CreateMemberViewModel, Member>(model);

            //var result = await _MemberRepository.AddAsync(member);
            _unitOfWork.GetRepository<Member>().Add(member); // add local
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            if (!members.Any()) return [];

            var membersViewModels = _mapper.Map<IEnumerable<Member>,IEnumerable<MemberViewModel>>(members);
            return membersViewModels;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int MemberId, CancellationToken ct = default)
        {
            var record = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(x => x.MemberId == MemberId, ct: ct);
            if (record == null) return null;

            return _mapper.Map<HealthRecord, HealthRecordViewModel>(record);
        }

        public async Task<MemberViewModel?> GetMemberDetailsByIdAsync(int MemberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId, ct);

            if (member == null) return null;

            var model = _mapper.Map<Member,MemberViewModel>(member);

            var activeMemberShip = await _unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(x => x.MemberId == MemberId && x.EndDate > DateTime.Now);

            if (activeMemberShip is not null)
            {
                var activePlan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(activeMemberShip.PlanId, ct);
                model.PlanName = activePlan?.Name;
                model.MemberShipStartDate = activeMemberShip.CreatedAt.ToString();
                model.MemberShipEndDate = activeMemberShip.EndDate.ToString();
            }

            return model;
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int MemberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId, ct);
            if (member == null) return null;

            return _mapper.Map<Member, MemberToUpdateViewModel>(member);
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member == null) return false;

            var EmailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == member.Email && x.Id != id);
            var PhoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == member.Phone && x.Id != id);
            if (EmailExists || PhoneExists) return false;

            _mapper.Map(model, member);

           _unitOfWork.GetRepository<Member>().Update(member); // update local
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;


        }

        public async Task<bool> RemoveMemberAsync(int MemberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId, ct);
            if (member == null) return false;

            var hasFutureBookings = await _unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == MemberId && b.Session.StartDate > DateTime.Now, ct);

            if (hasFutureBookings) return false;

           _unitOfWork.GetRepository<Member>().Delete(member); // delete local
            var result = await _unitOfWork.SaveChangesAsync(ct);
           return result > 0;
        }
    }
}
