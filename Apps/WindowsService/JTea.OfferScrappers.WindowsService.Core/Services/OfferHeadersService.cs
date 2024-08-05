using JTea.OfferScrappers.WindowsService.Core.Services.Interfaces;
using JTea.OfferScrappers.WindowsService.Models;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JTea.OfferScrappers.WindowsService.Models.Exceptions;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public class OfferHeadersService : IOfferHeadersService
    {
        private readonly IOfferHeadersRepository _offerHeadersRepository;
        private readonly IProcessingService _processingService;

        public OfferHeadersService(
            IProcessingService processingService,
            IOfferHeadersRepository offerHeadersRepository)
        {
            _offerHeadersRepository = offerHeadersRepository;
            _processingService = processingService;
        }

        public Result<OfferHeaderModel> Create(OfferHeaderModel offerHeader)
        {
            Result<OfferHeaderModel> result = CheckOfferHeaderExists(offerHeader);
            if (result.IsError) { return result; }

            OfferHeaderModel created = _offerHeadersRepository.Create(offerHeader);
            return new(created);
        }

        public Result Delete(int id)
        {
            if (_processingService.State == ProcessingState.Running) { return GetProcessingStateResult<bool>(); }

            bool deleted = _offerHeadersRepository.Delete(id);
            if (!deleted) { return Result.AsError(new OfferHeaderNotFoundException(id)); }

            return new();
        }

        public Result DeleteAll()
        {
            if (_processingService.State == ProcessingState.Running) { return Result.AsError(new ProcessingStateException()); }

            _offerHeadersRepository.DeleteAll();
            return new();
        }

        public List<OfferHeaderModel> GetAll() => _offerHeadersRepository.GetAll();

        public List<OfferHeaderModel> GetByFilter(OfferHeadersFilter filter) => _offerHeadersRepository.GetByFilter(filter);

        public Result<OfferHeaderModel> GetById(int id)
        {
            OfferHeaderModel result = _offerHeadersRepository.GetById(id);
            if (result == null) { return Result<OfferHeaderModel>.AsError(new OfferHeaderNotFoundException(id)); }

            return new(result);
        }

        public Result SetEnabled(int id, bool enabled)
        {
            if (_processingService.State == ProcessingState.Running) { return GetProcessingStateResult<bool>(); }

            bool updated = _offerHeadersRepository.SetEnabled(id, enabled);
            if (!updated) { return Result.AsError(new OfferHeaderNotFoundException(id)); }

            return new();
        }

        public Result<OfferHeaderModel> Update(UpdateOfferHeader updateOfferHeader)
        {
            if (_processingService.State == ProcessingState.Running) { return GetProcessingStateResult<OfferHeaderModel>(); }

            bool updated = _offerHeadersRepository.Update(updateOfferHeader);
            if (!updated) { return Result<OfferHeaderModel>.AsError(new OfferHeaderNotFoundException(updateOfferHeader.Id)); }

            return GetById(updateOfferHeader.Id);
        }

        private Result<OfferHeaderModel> CheckOfferHeaderExists(OfferHeaderModel offerHeader)
        {
            List<OfferHeaderModel> existingHeaders = _offerHeadersRepository.GetAll();
            if (existingHeaders.Any(x => x.Title == offerHeader.Title))
            {
                return Result<OfferHeaderModel>.AsError(new OfferHeaderExistsException(offerHeader.Title));
            }

            List<Scrapper> existingScrappers = existingHeaders.ConvertAll(x => ScrapperFactory.Create(x.Type, x.OfferUrl));

            Scrapper newScrapper = ScrapperFactory.Create(offerHeader.Type, offerHeader.OfferUrl);

            if (existingScrappers.Any(x => x.Type == newScrapper.Type && x.FullOfferUrl == newScrapper.FullOfferUrl))
            {
                return Result<OfferHeaderModel>.AsError(
                    new OfferHeaderExistsException(offerHeader.Type, offerHeader.OfferUrl));
            }

            return new();
        }

        private Result<T> GetProcessingStateResult<T>()
            => Result<T>.AsError(new ProcessingStateException());
    }
}