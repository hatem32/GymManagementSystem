using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class MemberShipRepository : GenericRepository<MemberShip>, IMemberShipRepository
    {
        private readonly GymDbContext _dbContext;

        public MemberShipRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MemberShip>> GetAllMembershipsWithMemberAndPlanAsync(Expression<Func<MemberShip, bool>>? predicate = null ,CancellationToken ct = default)
        {
            IQueryable<MemberShip> query = _dbContext.MemberShips.AsNoTracking().Include(m => m.Plan).Include(m => m.Member);

            if (predicate is not null) query = query.Where(predicate);

            return await query.ToListAsync(ct);
        }
    }
}
