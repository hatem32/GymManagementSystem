using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Enums;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("End date must be after start date.");
            if (model.StartDate <= DateTime.Now) return Result.Validation("Start date must be in the future.");
            if (model.Capacity < 1 || model.Capacity > 25) return Result.Validation("Capacity must be between 1 and 25.");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null) return Result.NotFound("Trainer not found.");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (category == null) return Result.NotFound("Category not found.");

            var isValid = Enum.TryParse<Specialty>(category.CategoryName, true, out var CategorySpecialty);
            if (!isValid || trainer.Specialty != CategorySpecialty) return Result.Validation("Cannot create this session for this trainer.");

            var Session = _mapper.Map<CreateSessionViewModel, Session>(model);

            _unitOfWork.GetRepository<Session>().Add(Session);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Failed to create session.");
        }

        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessionRepo = _unitOfWork.SessionRepository;
            var Sessions = await sessionRepo.GetAllSessionsWithTrainerAndCategoryAsync(ct);
            if (Sessions == null || !Sessions.Any()) return null;

            Sessions = Sessions.OrderByDescending(X => X.StartDate);
            var MappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(Sessions);

            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - await sessionRepo.GetCountOfBookedSlotsAsync(session.Id, ct);
            }

            return MappedSessions;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Category>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(result);
        }

        public async Task<Result<SessionViewModel>?> GetSessionByIdAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategoryAsync(SessionId, ct);
            if (session == null) return Result<SessionViewModel>.NotFound("Session Not Found");

            var mappedSession = _mapper.Map<Session, SessionViewModel>(session);
            mappedSession.AvailableSlots = mappedSession.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(SessionId, ct);

            return Result<SessionViewModel>.Ok(mappedSession);

        }
        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);
        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(SessionId, ct);
            if (session == null) return Result<UpdateSessionViewModel>.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
            {
                return Result<UpdateSessionViewModel>.Fail("Can Not Update Session That Has Already Started");
            }

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(SessionId, ct);

            if (bookingCount > 0)
            {
                return Result<UpdateSessionViewModel>.Fail("Can Not Update Session That Has Bookings");
            }

            var mappedSession = _mapper.Map<Session, UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.Ok(mappedSession);
        }


        public async Task<Result> UpdateSessionAsync(int SessionId, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(SessionId, ct);
            if (session == null) return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot edit a session that has already started.");

            if (model.EndDate <= model.StartDate) return Result.Validation("End date must be after start date.");

            var bookedCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(SessionId, ct);
             
            if (bookedCount > 0)
            {
                return Result.Fail("Can Not Update Session That Has Bookings");
            }

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start date must be in the future.");


            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null) return Result.NotFound("Trainer not found.");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId);

            var isValid = Enum.TryParse<Specialty>(category.CategoryName, true, out var CategorySpecialty);
            if (!isValid || trainer.Specialty != CategorySpecialty) return Result.Validation("Cannot create this session for this trainer.");


            _mapper.Map(model, session);
            session.UpdatedAt = DateTime.Now;
            _unitOfWork.SessionRepository.Update(session);

            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Update Session");
        }

        public async Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);

            if (session == null) return Result.NotFound("Session Not Found");

            if (session.EndDate >= DateTime.Now)
                return Result.Fail("Cannot delete a session that has not yet ended.");

            var bookedCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);
            if (bookedCount > 0)
                return Result.Fail("Cannot delete a session that has bookings.");


            _unitOfWork.SessionRepository.Delete(session); // delete local
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to Delete session.");
        }
    }
}
