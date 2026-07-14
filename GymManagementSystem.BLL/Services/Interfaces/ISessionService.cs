using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default!);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default!);
        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default);
        Task<Result<SessionViewModel>?> GetSessionByIdAsync(int SessionId, CancellationToken ct = default!);
        Task<Result<UpdateSessionViewModel>?> GetSessionToUpdateAsync(int  SessionId, CancellationToken ct = default!);
        Task<Result> UpdateSessionAsync(int SessionId, UpdateSessionViewModel model, CancellationToken ct = default!);
        Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct = default);
    }
}
