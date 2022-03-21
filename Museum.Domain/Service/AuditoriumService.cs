using Museum.Data.Entities;
using Museum.Domain.Interface;
using Museum.Domain.Models;
using Museum.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Museum.Domain.Common;

namespace Museum.Domain.Service
{
    public class AuditoriumService : IAuditoriumService
    {
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly IMuseumsRepository _museumsRepository;

        public AuditoriumService(IAuditoriumsRepository auditoriumsRepository, IMuseumsRepository museumsRepository)
        {
            _auditoriumsRepository = auditoriumsRepository;
            _museumsRepository = museumsRepository;
        }

        public async Task<CreateAuditoriumResultModel> CreateAuditorium(AuditoriumDomainModel domainModel)
        {
            var museum = await _auditoriumsRepository.GetByMuseumId(domainModel.MuseumId);

            var auditorium = await _auditoriumsRepository.GetByAuditName(domainModel.Name, domainModel.MuseumId);
            var sameAuditoriumName = auditorium.ToList();
            if (sameAuditoriumName != null && sameAuditoriumName.Count > 0)
            {
                return new CreateAuditoriumResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_SAME_NAME
                };
            }
            AuditoriumEntity newAuditorium = new AuditoriumEntity
            {
                Name = domainModel.Name,
                MuseumId = domainModel.MuseumId,
            };

            AuditoriumEntity insertedAuditorium = _auditoriumsRepository.Insert(newAuditorium);
            if (insertedAuditorium == null)
            {
                return new CreateAuditoriumResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_CREATION_ERROR
                };
            }

            _auditoriumsRepository.Save();
            CreateAuditoriumResultModel resultModel = new CreateAuditoriumResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Auditorium = new AuditoriumDomainModel
                {
                    Id = insertedAuditorium.Id,
                    Name = insertedAuditorium.Name,
                    MuseumId = insertedAuditorium.MuseumId,
                }
            };

            _auditoriumsRepository.Save();

            return resultModel;
        }
        public async Task<AuditoriumDomainModel> DeleteAuditorium(int id)
        {

         var deletedAuditorium = _auditoriumsRepository.Delete(id);
            if (deletedAuditorium == null)
            {
                return null;
            }

            _auditoriumsRepository.Save();

            AuditoriumDomainModel result = new AuditoriumDomainModel
            {
                MuseumId = deletedAuditorium.MuseumId,
                Id = deletedAuditorium.Id,
                Name = deletedAuditorium.Name,
            };

            return result;
        }

        public async Task<IEnumerable<AuditoriumDomainModel>> DeleteAuditoriumByMuseumId(int museumId)
        {
            var auditoriums = await _auditoriumsRepository.GetByMuseumId(museumId);

            if (auditoriums == null)
            {
                return null;
            }

            List<AuditoriumEntity> auditoriumList = auditoriums.ToList();

            List<AuditoriumDomainModel> deletedAuditoriums = new List<AuditoriumDomainModel>();

            foreach (AuditoriumEntity auditorium in auditoriumList)
            {

                var deletedAuditorium = _auditoriumsRepository.Delete(auditorium.Id);
                if (deletedAuditorium == null)
                {
                    return null;
                }

                AuditoriumDomainModel domainModel = new AuditoriumDomainModel
                {
                    MuseumId = deletedAuditorium.MuseumId,
                    Id = deletedAuditorium.Id,
                    Name = deletedAuditorium.Name
                };

                deletedAuditoriums.Add(domainModel);
            }

            return deletedAuditoriums;
        }

        public async Task<IEnumerable<AuditoriumDomainModel>> GetAllAsync()
        {
            var data = await _auditoriumsRepository.GetAll();

            if (data == null)
            {
                return null;
            }

            List<AuditoriumDomainModel> result = new List<AuditoriumDomainModel>();
            AuditoriumDomainModel model;
            foreach (var item in data)
            {
                model = new AuditoriumDomainModel
                {
                    Id = item.Id,
                    MuseumId = item.MuseumId,
                    Name = item.Name
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<AuditoriumDomainModel> GetAuditoriumByIdAsync(int id)
        {
            var data = await _auditoriumsRepository.GetByIdAsync(id);
            if (data == null)
            {
                return null;
            }
            
            AuditoriumDomainModel result = new AuditoriumDomainModel()
            {
                Id = data.Id,
                MuseumId = data.MuseumId,
                Name = data.Name,
            };

            return result;
        }

        public async Task<IEnumerable<AuditoriumDomainModel>> GetAuditoriumByMuseumId(int museumId)
        {
            var data = await _auditoriumsRepository.GetByMuseumId(museumId);

            if (data == null)
            {
                return null;
            }

            List<AuditoriumDomainModel> result = new List<AuditoriumDomainModel>();
            AuditoriumDomainModel model;
            foreach (var item in data)
            {
                model = new AuditoriumDomainModel
                {
                    Id = item.Id,
                    MuseumId = item.MuseumId,
                    Name = item.Name
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<AuditoriumDomainModel> UpdateAuditorium(AuditoriumDomainModel updateAuditorium)
        {
            AuditoriumEntity auditorium = new AuditoriumEntity()
            {
                Id = updateAuditorium.Id,
                MuseumId = updateAuditorium.MuseumId,
                Name = updateAuditorium.Name
            };

            var data = _auditoriumsRepository.Update(auditorium);

            if (data == null)
            {
                return null;
            }

            _auditoriumsRepository.Save();



            AuditoriumDomainModel domailModel = new AuditoriumDomainModel
            {
                Id = data.Id,
                Name = data.Name,
                MuseumId = data.MuseumId,
            };


            _auditoriumsRepository.Save();

            return domailModel;

    }
    }
}

