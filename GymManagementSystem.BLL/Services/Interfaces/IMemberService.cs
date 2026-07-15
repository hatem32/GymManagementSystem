using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberDetailsByIdAsync (int MemberId ,  CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int MemberId , CancellationToken ct = default);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int MemberId , CancellationToken ct = default);
        Task<Result> UpdateMemberDetailsAsync(int id , MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<Result> RemoveMemberAsync(int MemberId , CancellationToken ct = default);
    }
}
