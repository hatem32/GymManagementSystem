using GymManagementSystem.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Controllers
{
    public class PlansController : Controller
    {
        private readonly GymDbContext dbcontext;
        public PlansController()
        {
            dbcontext = new GymDbContext();
        }
        public async Task<IActionResult> Index()
        {
            var plans =await dbcontext.Plans.ToListAsync();
            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await dbcontext.Plans.FindAsync(id);
            if(plan is null)
                return RedirectToAction(nameof(Index));
            return View(plan);
        }
    }
}
