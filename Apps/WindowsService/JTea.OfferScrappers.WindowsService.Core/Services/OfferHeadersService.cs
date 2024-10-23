using JTea.OfferScrappers.WindowsService.Core.Services.Interfaces;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JTea.OfferScrappers.WindowsService.Models.Exceptions;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using JToolbox.Core.Models.Results;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public class OfferHeadersService : BaseService, IOfferHeadersService
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

        public Result Clear(int id)
        {
            if (_processingService.IsRunning) { return GetRunningStateResult(); }

            OfferHeaderModel offerHeader = _offerHeadersRepository.GetById(id, completeData: false);
            if (offerHeader == null) { return Result.AsError(new OfferHeaderNotFoundException(id)); }

            offerHeader.ClearProcessingData();
            _offerHeadersRepository.Clear(offerHeader);

            return new();
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
            if (_processingService.IsRunning) { return GetRunningStateResult(); }

            bool deleted = _offerHeadersRepository.Delete(id);
            if (!deleted) { return Result.AsError(new OfferHeaderNotFoundException(id)); }

            return new();
        }

        public Result DeleteAll()
        {
            if (_processingService.IsRunning) { return GetRunningStateResult(); }

            _offerHeadersRepository.DeleteAll();
            return new();
        }

        public List<OfferHeaderModel> GetAll() => _offerHeadersRepository.GetAll(completeData: false);

        public Result<OfferHeaderModel> GetById(int id)
        {
            OfferHeaderModel result = _offerHeadersRepository.GetById(id, completeData: false);
            if (result == null) { return Result<OfferHeaderModel>.AsError(new OfferHeaderNotFoundException(id)); }

            return new(result);
        }

        public Result SetEnabled(int id, bool enabled)
        {
            if (_processingService.IsRunning) { return GetRunningState<bool>(); }

            bool updated = _offerHeadersRepository.SetEnabled(id, enabled);
            if (!updated) { return Result.AsError(new OfferHeaderNotFoundException(id)); }

            return new();
        }

        public Result<OfferHeaderModel> Update(UpdateOfferHeaderModel updateOfferHeader)
        {
            if (_processingService.IsRunning) { return GetRunningState<OfferHeaderModel>(); }

            bool updated = _offerHeadersRepository.Update(updateOfferHeader);
            if (!updated) { return Result<OfferHeaderModel>.AsError(new OfferHeaderNotFoundException(updateOfferHeader.Id)); }

            return GetById(updateOfferHeader.Id);
        }

        private Result<OfferHeaderModel> CheckOfferHeaderExists(OfferHeaderModel offerHeader)
        {
            List<OfferHeaderModel> existingHeaders = _offerHeadersRepository.GetAll(completeData: false);
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
    }
}