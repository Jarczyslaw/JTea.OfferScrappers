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

        public OfferHeadersService(
            IOfferHeadersRepository offerHeadersRepository)
        {
            _offerHeadersRepository = offerHeadersRepository;
        }

        public Result<OfferHeaderModel> Create(OfferHeaderModel offerHeader)
        {
            Result<OfferHeaderModel> result = CheckOfferHeaderExists(offerHeader);
            if (result.IsError) { return result; }

            OfferHeaderModel created = _offerHeadersRepository.Create(offerHeader);
            return new(created);
        }

        public Result<bool> Delete(int id)
        {
            bool deleted = _offerHeadersRepository.Delete(id);
            if (!deleted) { return Result<bool>.AsError(new OfferHeaderNotFoundException(id)); }

            return new(true);
        }

        public void DeleteAll() => _offerHeadersRepository.DeleteAll();

        public List<OfferHeaderModel> GetAll() => _offerHeadersRepository.GetAll();

        public List<OfferHeaderModel> GetByFilter(OfferHeadersFilter filter) => _offerHeadersRepository.GetByFilter(filter);

        public Result<OfferHeaderModel> GetById(int id)
        {
            OfferHeaderModel result = _offerHeadersRepository.GetById(id);
            if (result == null) { return Result<OfferHeaderModel>.AsError(new OfferHeaderNotFoundException(id)); }

            return new(result);
        }

        public Result<bool> SetEnabled(int id, bool enabled)
        {
            bool updated = _offerHeadersRepository.SetEnabled(id, enabled);
            if (!updated) { return Result<bool>.AsError(new OfferHeaderNotFoundException(id)); }

            return new(true);
        }

        public Result<OfferHeaderModel> Update(UpdateOfferHeader updateOfferHeader)
        {
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
    }
}