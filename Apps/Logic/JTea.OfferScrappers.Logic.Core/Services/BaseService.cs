using JTea.OfferScrappers.Logic.Models.Exceptions;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.Logic.Core.Services
{
    public abstract class BaseService
    {
        protected static Result<T> GetProcessingStateResult<T>()
            => Result<T>.AsError(new ProcessingStateException());

        protected static Result GetProcessingStateResult()
            => Result.AsError(new ProcessingStateException());
    }
}