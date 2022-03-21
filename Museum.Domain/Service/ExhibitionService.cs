using Museum.Data.Entities;
using Museum.Domain.Common;
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
    public class ExhibitionService : IExhibitionService
    {
        private readonly IExhibitionRepository _exhibitionsRepository;
        private readonly IAuditoriumService _auditoriumService;

        public ExhibitionService(IExhibitionRepository exhibitionsRepository, IAuditoriumService auditoriumService)
        {
            _exhibitionsRepository = exhibitionsRepository;
            _auditoriumService = auditoriumService;
        }

        public async Task<CreateExhibitionResultModel> CreateExhibition(ExhibitionDomainModel domainModel)
        {
            int exhibitionTime = 6;

            var exhibitionsAtSameTime = _exhibitionsRepository.GetByAuditoriumId(domainModel.AuditoriumId)
                .Where(x => x.Opening < domainModel.Opening.AddHours(exhibitionTime) && x.Opening > domainModel.Opening.AddHours(-exhibitionTime))
                .ToList();

            if (exhibitionsAtSameTime != null && exhibitionsAtSameTime.Count > 0)
            {
                return new CreateExhibitionResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.EXHIBITIONS_AT_SAME_TIME
                };
            }

            var newExhibition = new ExhibitionEntity
            {
                ExhabitId = domainModel.ExhabitId,
                AuditoriumId = domainModel.AuditoriumId,
                Opening = domainModel.Opening,
                Name = domainModel.Name,
                Description = domainModel.Description,
                Image = domainModel.Image
            };

            var insertedExhibition = _exhibitionsRepository.Insert(newExhibition);

            if (insertedExhibition == null)
            {
                return new CreateExhibitionResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.EXHABIT_CREATION_ERROR
                };
            }

            _exhibitionsRepository.Save();
            CreateExhibitionResultModel result = new CreateExhibitionResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Exhibiition = new ExhibitionDomainModel
                {
                    Id = insertedExhibition.Id,
                    ExhabitId = insertedExhibition.ExhabitId,
                    AuditoriumId = insertedExhibition.AuditoriumId,
                    Opening = insertedExhibition.Opening,
                    Name = insertedExhibition.Name,
                    Description = insertedExhibition.Description,
                    Image = insertedExhibition.Image
                }
            };

            return result;
        }

        public async Task<ResponseModel<ExhibitionDomainModel>> Delete(Guid id)
        {
            var exhibition = await _exhibitionsRepository.GetByIdAsync(id);

            if (exhibition == null)
            {
                return new ResponseModel<ExhibitionDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Izlozba ne postoji"
                };
            }

            ExhibitionDomainModel exhibitionDomainModel = new ExhibitionDomainModel();
            exhibitionDomainModel.AuditoriumName = exhibition.Auditorium.Name;
            exhibitionDomainModel.AuditoriumId = exhibition.AuditoriumId;
            exhibitionDomainModel.Id = exhibition.Id;
            exhibitionDomainModel.Name = exhibition.Name;
            exhibitionDomainModel.Opening = exhibition.Opening;
            exhibitionDomainModel.Description = exhibition.Description;
            exhibitionDomainModel.Image = exhibition.Image;

            _exhibitionsRepository.Delete(id);
            _exhibitionsRepository.Save();
            return new ResponseModel<ExhibitionDomainModel>
            {
                IsSuccessful = true,
                DomainModel = exhibitionDomainModel
            };
        }

        public async Task<IEnumerable<ExhibitionDomainModel>> GetAllAsync()
        {
            var data = await _exhibitionsRepository.GetAll();

            if (data == null)
            {
                return null;
            }

            List<ExhibitionDomainModel> result = new List<ExhibitionDomainModel>();
            ExhibitionDomainModel model;
            foreach (var item in data)
            {
                model = new ExhibitionDomainModel
                {
                    Id = item.Id,
                    ExhabitId = item.ExhabitId,
                    AuditoriumId = item.AuditoriumId,
                    Opening = item.Opening,
                    Name = item.Name,
                    Description = item.Description,
                    Image = item.Image,
                    AuditoriumName = item.Auditorium.Name
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<ResponseModel<ExhibitionDomainModel>> GetExhibitionById(Guid id)
        {
            ExhibitionEntity result = await _exhibitionsRepository.GetByIdAsync(id);
            if (result == null)
            {
                return new ResponseModel<ExhibitionDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Izlozba ne postoji!"
                };
            }

            ExhibitionDomainModel exhibitionDomainModel = new ExhibitionDomainModel
            {
                Id = result.Id,
                AuditoriumId = result.AuditoriumId,
                ExhabitId = result.ExhabitId,
                AuditoriumName = result.Auditorium.Name,
                Opening = result.Opening,
                Name = result.Name,
                Description = result.Description,
                Image = result.Image,
            };
            return new ResponseModel<ExhibitionDomainModel>
            {
                DomainModel = exhibitionDomainModel,
                IsSuccessful = true
            };
        }

        public async Task<ResponseModel<ExhibitionDomainModel>> Update(ExhibitionDomainModel exhibitionDomainModel)
        {
            int exhibitionTime = 6;

            var exhibitionsAtSameTime = _exhibitionsRepository.GetByAuditoriumId(exhibitionDomainModel.AuditoriumId)
                .Where(x => x.Opening < exhibitionDomainModel.Opening.AddHours(exhibitionTime) && x.Opening > exhibitionDomainModel.Opening.AddHours(-exhibitionTime))
                .ToList();

            if (exhibitionsAtSameTime != null && exhibitionsAtSameTime.Count > 0)
            {
                return new ResponseModel<ExhibitionDomainModel>()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.EXHIBITIONS_AT_SAME_TIME,
                    DomainModel = null
                };
            }

            ExhibitionEntity exhibition = new ExhibitionEntity()
            {
                Id = exhibitionDomainModel.Id,
                AuditoriumId = exhibitionDomainModel.AuditoriumId,
                ExhabitId = exhibitionDomainModel.ExhabitId,
                Opening = exhibitionDomainModel.Opening,
                Name = exhibitionDomainModel.Name,
                Description = exhibitionDomainModel.Description,
                Image = exhibitionDomainModel.Image,
            };

            var data = _exhibitionsRepository.Update(exhibition);

            if (data == null)
            {
                return new ResponseModel<ExhibitionDomainModel>

                {
                    IsSuccessful = false,
                    ErrorMessage = "Update nije uspesno izvrsen."
                };
            }

            _exhibitionsRepository.Save();

            ExhibitionDomainModel domainModel = new ExhibitionDomainModel()
            {
                Id = data.Id,
                AuditoriumId = data.AuditoriumId,
                ExhabitId = data.ExhabitId,
                Opening = data.Opening,
                Name = data.Name,
                Description = data.Description,
                Image = data.Image,
            };

            return new ResponseModel<ExhibitionDomainModel>
            {
                IsSuccessful = true,
                DomainModel = domainModel
            };
        }
    }
}
