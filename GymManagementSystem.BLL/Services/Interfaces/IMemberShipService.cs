using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.MemberShipViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IMemberShipService
    {
        Task<IEnumerable<MemberShipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default);
        Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken ct = default);
        Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken ct = default);
        Task<Result> DeleteActiveMembershipAsync(int memberId, CancellationToken ct = default);
    }
}
