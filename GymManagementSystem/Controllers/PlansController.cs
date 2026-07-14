using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Controllers
{
    [Authorize]
    public class PlansController : Controller
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }



        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Plans = await _planService.GetAllPlansAsync(ct);
            return View(Plans);
        }




        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
           var Plan = await _planService.GetPlanByIdAsync(id , ct);
            if(Plan == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int id , CancellationToken ct)
        {
            var Plan = await _planService.GetPlanToUpdateAsync(id , ct);
            if(Plan == null)
            {
                TempData["ErrorMessage"] = "Plan cannot be edited (not found, inactive, or has active memberships).";
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(int id ,UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _planService.UpdatePlanAsync(id, model , ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Plan updated successfully.";
                return RedirectToAction(nameof(Index));
            }
          
            TempData["ErrorMessage"] = result.Error;
            return View(model);

            
        }




        [HttpPost]
        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            var result = await _planService.ToggleActivationAsync(id, ct);
            if (result.Success)
                TempData["SuccessMessage"] = "Plan status changed";
            else
                TempData["ErrorMessage"] = result.Error;
            return RedirectToAction(nameof(Index));
        }


    }
}
