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
    public class BookingRepository : GenericRepository<Booking> , IBookingRepository
    {
        private readonly GymDbContext _dbContext;

        public BookingRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Booking>> GetBySessionIdAsync(int sessionId, CancellationToken ct = default)
            => _dbContext.Bookings.AsNoTracking().Include(b => b.Member).Where(b => b.SessionId == sessionId).ToListAsync(ct);
    }
}
