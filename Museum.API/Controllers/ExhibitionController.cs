using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Museum.API.Models;
using Museum.Domain.Common;
using Museum.Domain.Interface;
using Museum.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Museum.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExhibitionController : ControllerBase
    {
        private readonly IExhibitionService _exhibitionService;

        public ExhibitionController(IExhibitionService exhibitionService)
        {
            _exhibitionService = exhibitionService;
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<ResponseModel<ExhibitionDomainModel>>> GetById(Guid id)
        {
            var response = await _exhibitionService.GetExhibitionById(id);
            if (!response.IsSuccessful)
                return BadRequest(new ErrorResponseModel()
                {
                    ErrorMessage = response.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            return Ok(response.DomainModel);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExhibitionDomainModel>>> GetAsync()
        {
            IEnumerable<ExhibitionDomainModel> exhibitionDomainModels;

            exhibitionDomainModels = await _exhibitionService.GetAllAsync();

            if (exhibitionDomainModels == null)
            {
                exhibitionDomainModels = new List<ExhibitionDomainModel>();
            }

            return Ok(exhibitionDomainModels);
        }


        [HttpPost]
        //[Authorize(Roles = "admin, super-user")]
        public async Task<ActionResult<ExhibitionDomainModel>> PostAsync(CreateExhibitionModel exhibitionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (exhibitionModel.Opening < DateTime.Now)
            {
                ModelState.AddModelError(nameof(exhibitionModel.Opening), Messages.EXHIBITIONS_IN_PAST);
                return BadRequest(new ErrorResponseModel()
                {
                    ErrorMessage = Messages.EXHIBITIONS_IN_PAST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }

            ExhibitionDomainModel domainModel = new ExhibitionDomainModel
            {
                AuditoriumId = exhibitionModel.AuditoriumId,
                Name = exhibitionModel.Name,
                Description = exhibitionModel.Description,
                Image = exhibitionModel.Image,
                ExhabitId = exhibitionModel.ExhabitId,
                Opening = exhibitionModel.Opening
            };

            CreateExhibitionResultModel createExhibitionResultModel;

            try
            {
                createExhibitionResultModel = await _exhibitionService.CreateExhibition(domainModel);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (!createExhibitionResultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = createExhibitionResultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Created("exhibitions//" + createExhibitionResultModel.Exhibiition.Id, createExhibitionResultModel.Exhibiition);
        }

        //[Authorize(Roles = "admin, super-user")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ResponseModel<ExhibitionDomainModel>>> DeleteById(Guid id)
        {
            ResponseModel<ExhibitionDomainModel> result = await _exhibitionService.Delete(id);
            if (result.IsSuccessful == false)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.DomainModel);
        }

        //[Authorize(Roles = "admin, super-user")]
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResponseModel<ExhibitionDomainModel>>> Update(Guid id, [FromBody] ExhibitionDomainModel exhibitionDomainModel)
        {
            if (exhibitionDomainModel.Opening < DateTime.Now)
            {
                ModelState.AddModelError(nameof(exhibitionDomainModel.Opening), Messages.EXHIBITIONS_IN_PAST);
                return BadRequest(new ErrorResponseModel()
                {
                    ErrorMessage = Messages.EXHIBITIONS_IN_PAST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }

            ResponseModel<ExhibitionDomainModel> exhibitionToUpdate = await _exhibitionService.GetExhibitionById(id);
            if (exhibitionToUpdate.IsSuccessful == false)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = exhibitionToUpdate.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }

            ResponseModel<ExhibitionDomainModel> result;

            exhibitionToUpdate.DomainModel.AuditoriumId = exhibitionDomainModel.AuditoriumId;
            exhibitionToUpdate.DomainModel.Name = exhibitionDomainModel.Name;
            exhibitionToUpdate.DomainModel.Description = exhibitionDomainModel.Description;
            exhibitionToUpdate.DomainModel.Image = exhibitionDomainModel.Image;
            exhibitionToUpdate.DomainModel.ExhabitId = exhibitionDomainModel.ExhabitId;
            exhibitionToUpdate.DomainModel.Opening = exhibitionDomainModel.Opening;

            try
            {
                result = await _exhibitionService.Update(exhibitionToUpdate.DomainModel);
            }

            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (result.IsSuccessful == false)
            {
                return BadRequest(new ErrorResponseModel
                {
                    ErrorMessage = result.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }

            return Ok(result.DomainModel);

        }

    }
}
