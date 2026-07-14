using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default!);
        Task<PlanViewModel?> GetPlanByIdAsync(int PlanId , CancellationToken ct = default!);
        Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int PlanId, CancellationToken ct = default!);
        Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default);
        Task<Result> ToggleActivationAsync(int planId, CancellationToken ct = default);
    }
}
