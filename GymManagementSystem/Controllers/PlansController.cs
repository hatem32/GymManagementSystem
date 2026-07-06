using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Controllers
{
    public class PlansController : Controller
    {
        //private readonly GymDbContext dbcontext
        //private readonly IPlanRepository planRepository = new PlanRepository() ;

        private readonly IGenericRepository<Plan> planRepository;

        public PlansController(IGenericRepository<Plan> planRepository)
        {
            this.planRepository = planRepository ;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await planRepository.GetAllAsync(ct: ct);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await planRepository.GetByIdAsync(id, ct);
            if (plan is null)
                return RedirectToAction(nameof(Index));
            return View(plan);
        }
    }
}
