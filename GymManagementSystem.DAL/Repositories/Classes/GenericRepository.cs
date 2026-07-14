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
        public void Add(TEntity entity)
        {
            _Set.Add(entity);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> Predicate, CancellationToken ct = default)
        {
            return _Set.AsNoTracking().AnyAsync(Predicate, ct);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
        {
            return predicate is null ? _Set.AsNoTracking().CountAsync(ct) : _Set.AsNoTracking().CountAsync(predicate, ct);
        }

        public void Delete(TEntity entity)
        {
            _Set.Remove(entity);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> Predicate, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _Set : _Set.AsNoTracking();
            return await query.FirstOrDefaultAsync(Predicate);

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? Predicate, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _Set : _Set.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _Set.FindAsync(id, ct);
        }

        public void Update(TEntity entity)
        {
            _Set.Update(entity);
        }
    }
}
