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
    [ApiController]
    [Route("api/[controller]")]
    public class MuseumsController : ControllerBase
    {
        private readonly IMuseumService _museumService;

        public MuseumsController(IMuseumService museumService)
        {
            _museumService = museumService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MuseumDomainModel>>> GetAsync()
        {
            IEnumerable<MuseumDomainModel> cinemaDomainModels;

            cinemaDomainModels = await _museumService.GetAllAsync();

            if (cinemaDomainModels == null)
            {
                cinemaDomainModels = new List<MuseumDomainModel>();
            }

            return Ok(cinemaDomainModels);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<MuseumDomainModel>> GetByIdAsync(int id)
        {
            MuseumDomainModel museum;

            museum = await _museumService.GetMuseumByIdAsync(id);

            if (museum == null)
            {
                return NotFound(Messages.MUSEUM_DOES_NOT_EXIST);
            }

            return Ok(museum);
        }
        [HttpDelete]
        [Route("{museumId}")]
        public async Task<ActionResult> Delete(int museumId)
        {
            MuseumDomainModel deletedMuseum;

            try
            {
                deletedMuseum = await _museumService.DeleteMuseum(museumId);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MUSEUM_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest

                };
                return BadRequest(errorResponse);
            }
            catch (ArgumentNullException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MUSEUM_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (deletedMuseum == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MUSEUM_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted("museums/" + deletedMuseum.Id, deletedMuseum);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateMuseumModel museumModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MuseumDomainModel museum = new MuseumDomainModel()
            {
                Name = museumModel.Name,
                City = museumModel.City,
                Address = museumModel.Address,
                Email = museumModel.Email,
                Phone = museumModel.Phone
            };


            CreateMuseumResultModel createMuseum;
            try
            {
                createMuseum = await _museumService.AddMuseum(museum);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (createMuseum.IsSuccessful != true)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = createMuseum.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Created("museums//" + createMuseum.Museum.Id, createMuseum.Museum);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CreateMuseumModel museumModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MuseumDomainModel updateMuseum;

            updateMuseum = await _museumService.GetMuseumByIdAsync(id);

            if (updateMuseum == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MUSEUM_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            updateMuseum.Name = museumModel.Name;
            updateMuseum.City = museumModel.City;
            updateMuseum.Address = museumModel.Address;
            updateMuseum.Email = museumModel.Email;
            updateMuseum.Phone = museumModel.Phone;

            MuseumDomainModel museumDomainModel;
            try
            {
                museumDomainModel = await _museumService.UpdateMuseum(updateMuseum);
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

            return Accepted("museums//" + museumDomainModel.Id, museumDomainModel);

        }
    }
}
