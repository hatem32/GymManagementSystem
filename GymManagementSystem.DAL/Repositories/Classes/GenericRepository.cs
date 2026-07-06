using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;
        private readonly DbSet<TEntity> _Set;
        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            _Set = dbContext.Set<TEntity>();
        }
        public async Task<int> AddAsync(TEntity entity, CancellationToken ct = default)
        {
            _Set.Add(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> Predicate, CancellationToken ct = default)
        {
            return _Set.AsNoTracking().AnyAsync(Predicate, ct);
        }

        public async Task<int> DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            _Set.Remove(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _Set : _Set.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _Set.FindAsync(id, ct);
        }

        public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            _Set.Update(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }
    }
}
