using GymManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Models
{
    public class MemberShip : BaseEntity
    {
        public Member Member { get; set; } = default!;

        public int MemberId { get; set; }

        public Plan Plan { get; set; } = default!;
        public int PlanId { get; set; }

        // StartDate = CreatedAt of BaseEntity
        public  DateTime EndDate { get; set; }

        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
        private bool IsActive => EndDate > DateTime.Now;
    }
}
