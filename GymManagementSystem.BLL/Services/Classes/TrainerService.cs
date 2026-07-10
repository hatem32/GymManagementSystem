using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModel;
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
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            //check email
            var EmailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email, ct);
            //check phone
            var PhoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone, ct);

            //email and phone exists return false

            if (EmailExists || PhoneExists) return false;

            //else return true and add member
            var trainer = new Trainer()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Specialty = model.Specialties,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    City = model.City,
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street
                }
            };
            _unitOfWork.GetRepository<Trainer>().Add(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            return trainers.Select(t => new TrainerViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialties = t.Specialty.ToString()
            });

        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer == null) return null;
            return new TrainerViewModel()
            {
                Name = trainer.Name,
                Specialties = trainer.Specialty.ToString(),
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}"
            };
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer == null) return null;
            return new TrainerToUpdateViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialty = trainer.Specialty
            };
        }   

        public async Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null) return false;

            var hasFutureSessions = await _unitOfWork.GetRepository<Session>().AnyAsync(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now, ct);
            if (hasFutureSessions)
                return false;

            _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer == null) return false;

            var EmailExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == trainer.Email && x.Id != trainerId , ct);
            var PhoneExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == trainer.Phone && x.Id != trainerId, ct);
            if (EmailExists || PhoneExists) return false;

            trainer.Phone = model.Phone;
            trainer.Address.City = model.City;
            trainer.Email = model.Email;
            trainer.Address.Street = model.Street;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Specialty = model.Specialty;
            trainer.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Trainer>().Update(trainer );
            var result = await _unitOfWork.SaveChangesAsync(ct);  
            return result > 0;
        }
    }
}
