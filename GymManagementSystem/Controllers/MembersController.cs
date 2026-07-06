using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        //Get  BaseUrl/Members/Index
        //Index - list all members

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMembersAsync(ct);
            return View(members);
        }

        //Get  BaseUrl/Members/MemberDetails/{id}
        //MemberDetails - Show one member's details


        //Get  BaseUrl/Members/HealthRecordDetails/{id}
        //HealthRecordDetails - Show one member's details


        #region Create Member

        //Get  BaseUrl/Members/Create
        //Create - Show empty form
        [HttpGet]
        public IActionResult Create() => View();

        //Post  BaseUrl/Members/Create {Member}
        //CreateMember - submit form
       
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model , CancellationToken ct)
        {
            if(!ModelState.IsValid) return View(nameof(Create) , model);
            var result = await _memberService.CreateMemberAsync(model, ct);
            if (result)
                TempData["SuccessMessage"] = "Member Created Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Create Member";
            return RedirectToAction(nameof(Index));

        }
        #endregion


        #region Edit Member

        //Get  BaseUrl/Members/Edit/{id}
        //Edit - Displays Edit Form


        //Post  BaseUrl/Members/Edit {Member}
        //Edit - submit form


        #endregion


        #region Delete Member

        //Get  BaseUrl/Members/Delete/{id}
        //Delete - show confirmation form


        //Post  BaseUrl/Members/DeleteConfirmed/{id}
        //DeleteConfirmed - Delete after confirmation


        #endregion
    }
}
