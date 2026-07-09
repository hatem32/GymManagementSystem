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
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            //Get member by id 
            var member = await _memberService.GetMemberDetailsByIdAsync(id , ct);

            //check member is null => return index with message 
            if(member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            //member is not null => return view data
            return View(member);

        }




        //Get  BaseUrl/Members/HealthRecordDetails/{id}
        //HealthRecordDetails - Show one member's details
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            //Get health record  by id 
            var record = await _memberService.GetMemberHealthRecordAsync(id , ct);
            //check is null => return index with message 
            if (record == null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }

            //record is not null => return view data
            return View(record);
        }




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

        [HttpGet]
        public async Task<IActionResult> EditMember(int id , CancellationToken ct)
        {
            var member = await _memberService.GetMemberToUpdateAsync(id , ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        

        //Post  BaseUrl/Members/Edit {Member}
        //Edit - submit form

        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute] int id , MemberToUpdateViewModel model ,  CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var result =await _memberService.UpdateMemberDetailsAsync(id , model , ct);
            if (result)
                TempData["SuccessMessage"] = "Member Updated Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Update Member";

            return RedirectToAction(nameof(Index));
        }

        #endregion


        #region Delete Member

        //Get  BaseUrl/Members/Delete/{id}
        //Delete - show confirmation form
        public async Task<IActionResult> Delete (int id , CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id , ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        //Post  BaseUrl/Members/DeleteConfirmed/{id}
        //DeleteConfirmed - Delete after confirmation
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id , CancellationToken ct)
        {
            var result = await _memberService.RemoveMemberAsync(id , ct);
            if (result)
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Delete Member";

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
