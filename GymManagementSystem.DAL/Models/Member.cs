using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }

        //JoinDate = CreatedAt of BaseEntity

        public HealthRecord HealthRecord { get; set; } = default!;

        public ICollection<MemberShip> MemberShips { get; set; } = default!;
        public ICollection<Booking> MemberSessions { get; set; } = default!;


    }
}
