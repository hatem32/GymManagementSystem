using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var Plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            if (!Plans.Any()) return [];
            return Plans.Select(x => new PlanViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                DurationDays = x.DurationDays,
                IsActive = x.IsActive,
                Price = x.Price,
            });
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int PlanId, CancellationToken ct = default)
        {
            var Plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(PlanId, ct: ct);
            if (Plan == null) return null;
            return new PlanViewModel()
            {
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                IsActive = Plan.IsActive,
                Price = Plan.Price,
            };

        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int PlanId, CancellationToken ct = default)
        {
            var Plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(PlanId, ct);
            if (Plan == null || !Plan.IsActive) return null;
            if (await HasActiveMembershipsAsync(PlanId, ct)) return null;
            return new UpdatePlanViewModel()
            {
                PlanName = Plan.Name,
                Price = Plan.Price,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
            };
        }

        public async Task<bool> ToggleActivationAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
            if (plan is null) return false;

            if (plan.IsActive && await HasActiveMembershipsAsync(planId, ct))
                return false;

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
           
            return result > 0;

        }

        public async Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var Plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (Plan == null) return false;
            if (await HasActiveMembershipsAsync(id, ct)) return false;

            Plan.Price = model.Price;
            Plan.Description = model.Description;
            Plan.DurationDays = model.DurationDays;
            Plan.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Plan>().Update(Plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        #region Helper Methods

        private async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken ct)
        {
            return await _unitOfWork.GetRepository<MemberShip>().AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);
        }

        #endregion
    }
}
