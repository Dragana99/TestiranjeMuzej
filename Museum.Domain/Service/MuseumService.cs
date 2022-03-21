using Museum.Data.Entities;
using Museum.Domain.Interface;
using Museum.Domain.Models;
using Museum.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Museum.Domain.Common;

namespace Museum.Domain.Service
{
    public class MuseumService : IMuseumService
    {
        private readonly IMuseumsRepository _museumsRepository;
        private readonly IAuditoriumService _auditoriumService;
        public MuseumService(IMuseumsRepository museumsRepository, IAuditoriumService auditoriumService)
        {
            _museumsRepository = museumsRepository;
            _auditoriumService = auditoriumService;
        }

        public async Task<CreateMuseumResultModel> AddMuseum(MuseumDomainModel newMuseum)
        {
            var museum = await _museumsRepository.GetByMuseumName(newMuseum.Name);
            if (museum != null)
            {
                return new CreateMuseumResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MUSEUM_SAME_NAME
                };
            }

            MuseumEntity createMuseum = new MuseumEntity()
            {
                Name = newMuseum.Name,
                Address = newMuseum.Address,
                City = newMuseum.City,
                Email = newMuseum.Email,
                Phone = newMuseum.Phone
            };

            var data = _museumsRepository.Insert(createMuseum);
            if (data == null)
            {
                return new CreateMuseumResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MUSEUM_CREATION_ERROR
                };
            }

            _museumsRepository.Save();

            MuseumDomainModel museumDModel = new MuseumDomainModel()
            {
                Id = data.Id,
                Name = data.Name,
                Address = data.Address,
                City = data.City,
                Email = data.Email,
                Phone = data.Phone
            };

            CreateMuseumResultModel museumResultModel = new CreateMuseumResultModel()
            {
                ErrorMessage = null,
                IsSuccessful = true,
                Museum = museumDModel
            };

            return museumResultModel;
        }

        public async Task<MuseumDomainModel> DeleteMuseum(int museumId)
        {
            var deletedAuditoriums = await _auditoriumService.DeleteAuditoriumByMuseumId(museumId);

            if (deletedAuditoriums == null)
            {
                return null;
            }

            var deletedMuseum = _museumsRepository.Delete(museumId);
            if (deletedMuseum == null)
            {
                return null;
            }

            _museumsRepository.Save();

            MuseumDomainModel result = new MuseumDomainModel
            {
                Id = deletedMuseum.Id,
                Name = deletedMuseum.Name,
                Address = deletedMuseum.Address,
                City = deletedMuseum.City,
                Email = deletedMuseum.Email,
                Phone = deletedMuseum.Phone
            };

            return result;
        }

        public async Task<IEnumerable<MuseumDomainModel>> GetAllAsync()
        {
            var data = await _museumsRepository.GetAll();

            if (data == null)
            {
                return null;
            }

            List<MuseumDomainModel> result = new List<MuseumDomainModel>();
            MuseumDomainModel model;
            foreach (var item in data)
            {
                model = new MuseumDomainModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Address = item.Address,
                    City = item.City,
                    Email = item.Email,
                    Phone = item.Phone
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<MuseumDomainModel> GetMuseumByIdAsync(int id)
        {
            var data = await _museumsRepository.GetByIdAsync(id);

            if (data == null)
            {
                return null;
            }

            MuseumDomainModel domainModel = new MuseumDomainModel
            {
                Id = data.Id,
                Name = data.Name,
                Address = data.Address,
                City = data.City,
                Email = data.Email,
                Phone = data.Phone
            };

            return domainModel;
        }

        public async Task<MuseumDomainModel> UpdateMuseum(MuseumDomainModel updateMuseum)
        {
            MuseumEntity cinema = new MuseumEntity()
            {
                Id = updateMuseum.Id,
                Name = updateMuseum.Name,
                Address = updateMuseum.Address,
                City = updateMuseum.City,
                Email = updateMuseum.Email,
                Phone = updateMuseum.Phone
            };

            var data = _museumsRepository.Update(cinema);

            if (data == null)
            {
                return null;
            }
            _museumsRepository.Save();

            MuseumDomainModel domainModel = new MuseumDomainModel()
            {
                Id = data.Id,
                Name = data.Name,
                Address = data.Address,
                City = data.City,
                Email = data.Email,
                Phone = data.Phone
            };

            return domainModel;
        }
    }
}
