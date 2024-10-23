using JTea.OfferScrappers.WindowsService.Models.Exceptions;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public abstract class BaseService
    {
        protected static Result<T> GetRunningState<T>()
            => Result<T>.AsError(new ProcessingStateException());

        protected static Result GetRunningStateResult()
            => Result.AsError(new ProcessingStateException());
    }
}