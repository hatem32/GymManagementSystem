using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<string, object> _repositories = [];

        public UnitOfWork( GymDbContext dbContext , ISessionRepository sessionRepository , IMemberShipRepository memberShipRepository , IBookingRepository bookingRepository)
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
            MemberShipRepository = memberShipRepository;
            BookingRepository = bookingRepository;
        }

        public ISessionRepository SessionRepository { get; }

        public IMemberShipRepository MemberShipRepository { get; }

        public IBookingRepository BookingRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            // check entity ==  ??????
            var typeName = typeof(TEntity).Name;

            //if exists -> return
            if(_repositories.TryGetValue(typeName , out object? value))
            {
                return (IGenericRepository<TEntity>)value;
            }
            //if not exists -> create - store - return
            else
            {
                var Repo = new GenericRepository<TEntity>(_dbContext);
                _repositories[typeName] = Repo;
                return Repo;
            }

        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _dbContext.SaveChangesAsync(ct);
        }
    }
}
