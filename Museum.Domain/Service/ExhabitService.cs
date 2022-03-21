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
    public class ExhabitService : IExhabitService
    {
        private readonly IExhabitRepository _exhabitRepository;
        private readonly IExhibitionRepository _exhibitionRepository;

        public ExhabitService(IExhabitRepository exhabitRepository, IExhibitionRepository exhibitionRepository)
        {
            _exhabitRepository = exhabitRepository;
            _exhibitionRepository = exhibitionRepository;
        }

        public async Task<ResponseModel<ExhabitDomainModel>> AddExhabit(ExhabitDomainModel newExhabit)
        {
            ExhabitEntity exhabitToCreate = new ExhabitEntity()
            {
                Name = newExhabit.Name,
                Picture = newExhabit.Picture,
                Year = newExhabit.Year
            };

            var data = _exhabitRepository.Insert(exhabitToCreate);
            if (data == null)
            {
                return new ResponseModel<ExhabitDomainModel>
                {
                    ErrorMessage = "Dodavanje izlozbe nije uspesno izvrseno!",
                    IsSuccessful = false
                };
            }

            _exhabitRepository.Save();

            ExhabitDomainModel domainModel = new ExhabitDomainModel()
            {
                Id = data.Id,
                Name = data.Name,
                Picture = data.Picture,
                Year = data.Year
            };

            return new ResponseModel<ExhabitDomainModel>
            {
                DomainModel = domainModel,
                IsSuccessful = true
            };
        }

        public async Task<ResponseModel<ExhabitDomainModel>> DeleteExhabit(Guid id)
        {
            var checkExhibitions = _exhibitionRepository.GetByExhabitId(id);

            foreach (var item in checkExhibitions)
            {
                if (item.Opening > DateTime.Now)
                {
                    return new ResponseModel<ExhabitDomainModel>
                    {
                        IsSuccessful = false,
                        ErrorMessage = "Eksponat ce piti predstvljen na narednoj izlozbi!"
                    };
                }
            }

            var data = _exhabitRepository.Delete(id);

            if (data == null)
            {
                return null;
            }

            _exhabitRepository.Save();

            ExhabitDomainModel domainModel = new ExhabitDomainModel
            {
                Id = data.Id,
                Name = data.Name,
                Picture = data.Picture,
                Year = data.Year

            };

            return new ResponseModel<ExhabitDomainModel>
            {
                DomainModel = domainModel,
                IsSuccessful = true
            };
        }

        public async Task<IEnumerable<ExhabitDomainModel>> GetAllExhabits()
        {
            var data = await _exhabitRepository.GetAll();

            var result = new List<ExhabitDomainModel>();
            foreach (var item in data)
            {
                result.Add(
                    new ExhabitDomainModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Picture = item.Picture,
                        Year = item.Year
                    });
            }
            return result;
        }

        public async Task<ResponseModel<ExhabitDomainModel>> GetExhabitByIdAsync(Guid id)
        {
            var data = await _exhabitRepository.GetByIdAsync(id);

            if (data == null)
            {
                return new ResponseModel<ExhabitDomainModel>
                {
                    ErrorMessage = "Eksponat ne postoji!",
                    IsSuccessful = false
                };
            }

            ExhabitDomainModel domainModel = new ExhabitDomainModel
            {
                Id = data.Id,
                Name = data.Name,
                Picture = data.Picture,
                Year = data.Year
            };

            return new ResponseModel<ExhabitDomainModel>
            {
                DomainModel = domainModel,
                IsSuccessful = true
            };
        }

        public async Task<ResponseModel<IEnumerable<ExhabitDomainModel>>> GetExhabitsByAuditoriumId(int id)
        {
            var exhibitions = _exhibitionRepository.GetByAuditoriumId(id);

            if (exhibitions == null)
                return new ResponseModel<IEnumerable<ExhabitDomainModel>>()
                {
                    DomainModel = null,
                    ErrorMessage = Messages.EXHABIT_GET_ALL_EXHABIT_ERROR,
                    IsSuccessful = false
                };
            var exhabits = exhibitions.Select(exhabit => new ExhabitDomainModel()
            {
                Id = exhabit.Exhabit.Id,
                Name = exhabit.Exhabit.Name,
                Picture = exhabit.Exhabit.Picture,
                Year = exhabit.Exhabit.Year,
            }).ToList();

            return new ResponseModel<IEnumerable<ExhabitDomainModel>>()
            {
                ErrorMessage = null,
                IsSuccessful = true,
                DomainModel = exhabits
            };
        }

        public async Task<ResponseModel<ExhabitDomainModel>> UpdateExhabit(ExhabitDomainModel updateExhabit)
        {
            var movieToUpdate = await _exhabitRepository.GetByIdAsync(updateExhabit.Id);
            if (movieToUpdate == null)
            {
                return new ResponseModel<ExhabitDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Greska pri update-u"
                };
            }
            ExhabitEntity exhabit = new ExhabitEntity()
            {
                Id = updateExhabit.Id,
                Name = updateExhabit.Name,
                Picture = updateExhabit.Picture,
                Year = updateExhabit.Year
            };

            var data = _exhabitRepository.Update(exhabit);

            if (data == null)
            {
                return new ResponseModel<ExhabitDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Greska za update eksponata"
                };
            }

            _exhabitRepository.Save();

            ExhabitDomainModel domainModel = new ExhabitDomainModel()
            {
                Id = data.Id,
                Name = data.Name,
                Picture = data.Picture,
                Year = data.Year
            };

            return new ResponseModel<ExhabitDomainModel>
            {
                IsSuccessful = true,
                DomainModel = domainModel
            };
        }
    }
}
