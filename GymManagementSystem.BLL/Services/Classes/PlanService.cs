using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Http;
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
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var Plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            if (!Plans.Any()) return [];
            return _mapper.Map<IEnumerable<PlanViewModel>>(Plans);
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int PlanId, CancellationToken ct = default)
        {
            var Plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(PlanId, ct: ct);
            if (Plan is null) return null;
            return _mapper.Map<PlanViewModel>(Plan);
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int PlanId, CancellationToken ct = default)
        {
            var Plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(PlanId, ct);
            if (Plan == null || !Plan.IsActive) return null;
            if (await HasActiveMembershipsAsync(PlanId, ct)) return null;
            return _mapper.Map<UpdatePlanViewModel>(Plan);
        }

        public async Task<Result> ToggleActivationAsync(int planId, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Plan>();
            var plan = await repo.GetByIdAsync(planId, ct);
            if (plan is null) return Result.NotFound("Plan not found.");

            if (plan.IsActive && await HasActiveMembershipsAsync(planId, ct))
                return Result.Fail("Cannot deactivate a plan that has active memberships.");

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;
            repo.Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to Toggle Plan Status");

        }

        public async Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Plan>();
            var plan = await repo.GetByIdAsync(id, ct);
            if (plan is null) return Result.NotFound("Plan not found.");
            if (await HasActiveMembershipsAsync(id, ct))
                return Result.Fail("Cannot edit a plan that has active memberships.");

            _mapper.Map(model, plan);
            plan.UpdatedAt = DateTime.Now;
            repo.Update(plan);
            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();
        }

        #region Helper Methods

        private async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken ct)
        {
            return await _unitOfWork.GetRepository<MemberShip>().AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);
        }

        #endregion
    }
}
