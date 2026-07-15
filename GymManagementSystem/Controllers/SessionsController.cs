using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.PL.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Sessions = await _sessionService.GetAllSessionsAsync(ct);
            return View(Sessions);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionByIdAsync(id, ct);
            if (result.Success)
            {
                return View(result.Value);

            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }
        }


        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(model);
            }

            var result = await _sessionService.CreateSessionAsync(model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session Created";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;
            await PopulateDropdownsAsync();
            return View(model);
        }

       

        #endregion


        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionToUpdateAsync(id, ct);
            if (result.Success)
            {
                await PopulateDropdownsAsync();
                return View(result.Value);
            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(model);
            }

            var result = await _sessionService.UpdateSessionAsync(id, model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session Updated";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(), "Id", "Name");
                return View(model);

            }
        }

        #endregion


        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionByIdAsync(id);
            if (result.Success)
            {
                return View(result.Value);
            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _sessionService.RemoveSessionAsync(id);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session Deleted";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        private async Task PopulateDropdownsAsync()
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesForDropDownAsync(), "Id", "CategoryName");
        }
    }
}