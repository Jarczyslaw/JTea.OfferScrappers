﻿using JTea.OfferScrappers.WindowsService.Models;
using JTea.OfferScrappers.WindowsService.Models.Domain;
using JTea.OfferScrappers.WindowsService.Models.Exceptions;
using JTea.OfferScrappers.WindowsService.Persistence.Abstraction;
using MapsterMapper;

namespace JTea.OfferScrappers.WindowsService.Core.Services
{
    public class OfferHeadersService : IOfferHeadersService
    {
        private readonly IMapper _mapper;
        private readonly IOfferHeadersRepository _offerHeadersRepository;

        public OfferHeadersService(
            IMapper mapper,
            IOfferHeadersRepository offerHeadersRepository)
        {
            _mapper = mapper;
            _offerHeadersRepository = offerHeadersRepository;
        }

        public OfferHeaderModel Create(OfferHeaderModel offerHeader)
        {
            if (_offerHeadersRepository.CheckIfOfferHeaderExists(offerHeader))
            {
                throw new OfferHeaderExistsException(offerHeader.Type, offerHeader.OfferUrl);
            }

            return _offerHeadersRepository.Create(offerHeader);
        }

        public bool Delete(int id)
        {
            bool deleted = _offerHeadersRepository.Delete(id);

            if (!deleted) { throw new OfferHeaderNotFoundException(id); }
            return deleted;
        }

        public void DeleteAll() => _offerHeadersRepository.DeleteAll();

        public List<OfferHeaderModel> GetAll() => _offerHeadersRepository.GetAll();

        public List<OfferHeaderModel> GetByFilter(OfferHeadersFilter filter) => _offerHeadersRepository.GetByFilter(filter);

        public OfferHeaderModel GetById(int id)
        {
            OfferHeaderModel result = _offerHeadersRepository.GetById(id);
            if (result == null) { throw new OfferHeaderNotFoundException(id); }

            return result;
        }

        public bool SetEnabled(int id, bool enabled)
        {
            bool updated = _offerHeadersRepository.SetEnabled(id, enabled);
            if (!updated) { throw new OfferHeaderNotFoundException(id); }

            return updated;
        }

        public OfferHeaderModel Update(UpdateOfferHeader updateOfferHeader)
        {
            bool updated = _offerHeadersRepository.Update(updateOfferHeader);
            if (!updated) { throw new OfferHeaderNotFoundException(updateOfferHeader.Id); }

            return GetById(updateOfferHeader.Id);
        }
    }
}