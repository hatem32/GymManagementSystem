using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct = default)
        {
            var query = _dbContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category);

            return await query.ToListAsync(ct);
        }

        public async Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default)
        {
            return await _dbContext.Bookings.AsNoTracking().CountAsync(c => c.SessionId == sessionId);
        }

        public async Task<Session?> GetSessionWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct = default)
        {
            return await _dbContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == sessionId, ct);

        }
    }
}
