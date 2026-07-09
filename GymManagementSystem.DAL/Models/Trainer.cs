using GymManagementSystem.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Models
{
    public class Trainer : GymUser
    {
        public Specialty Specialty { get; set; }

        // HireDate = CreatedAt of BaseEntity

        public ICollection<Session> sessions { get; set; } = default!;
    }
}
