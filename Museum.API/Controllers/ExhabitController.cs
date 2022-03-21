using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class ExhabitController : ControllerBase
    {
        private readonly IExhabitService _exhabitService;

        private readonly ILogger<ExhabitController> _logger;

        public ExhabitController(ILogger<ExhabitController> logger, IExhabitService exhabitService)
        {
            _logger = logger;
            _exhabitService = exhabitService;
        }

        [HttpGet]
        [Route("auditoriumId/{id}")]
        public async Task<ActionResult<ResponseModel<IEnumerable<ExhabitDomainModel>>>> GetExhabitsByAuditoriumId(int id)
        {
            var movies = await _exhabitService.GetExhabitsByAuditoriumId(id);
            if (!movies.IsSuccessful)
                return NotFound(new ErrorResponseModel()
                {
                    ErrorMessage = movies.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.NotFound
                });

            return Ok(movies.DomainModel);
        }

        /// <summary>
        /// Gets Movie by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ResponseModel<ExhabitDomainModel>>> GetAsync(Guid id)
        {
            ResponseModel<ExhabitDomainModel> movie;

            movie = await _exhabitService.GetExhabitByIdAsync(id);

            if (movie.IsSuccessful == false)
            {
                return NotFound(movie.ErrorMessage);
            }

            return Ok(movie.DomainModel);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExhabitDomainModel>>> GetAll()
        {
            IEnumerable<ExhabitDomainModel> movieDomainModels;

            movieDomainModels = await _exhabitService.GetAllExhabits();

            if (movieDomainModels == null)
            {
                movieDomainModels = new List<ExhabitDomainModel>();
            }

            return Ok(movieDomainModels);
        }

        //[Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<ResponseModel<ExhabitDomainModel>>> Post([FromBody] CreateExhabitModel exhabitModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ExhabitDomainModel domainModel = new ExhabitDomainModel
            {
                Name = exhabitModel.Name,
                Picture = exhabitModel.Picture,
                Year = exhabitModel.Year 
            };

            ResponseModel<ExhabitDomainModel> createExhabit;

            try
            {
                createExhabit = await _exhabitService.AddExhabit(domainModel);
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

            if (createExhabit == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.EXHABIT_CREATION_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            if (createExhabit.IsSuccessful == false)
            {
                return BadRequest(createExhabit.ErrorMessage);
            }

            return Created("exhabit//" + createExhabit.DomainModel.Id, createExhabit.DomainModel);
        }

        //[Authorize(Roles = "admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResponseModel<ExhabitDomainModel>>> Put(Guid id, [FromBody] CreateExhabitModel exhabitModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResponseModel<ExhabitDomainModel> exhabitToUpdate;

            exhabitToUpdate = await _exhabitService.GetExhabitByIdAsync(id);

            if (exhabitToUpdate == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.EXHABIT_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            exhabitToUpdate.DomainModel.Name = exhabitModel.Name;
            exhabitToUpdate.DomainModel.Picture = exhabitModel.Picture;
            exhabitToUpdate.DomainModel.Year = exhabitModel.Year;

            ResponseModel<ExhabitDomainModel> exhabitDomainModel;
            try
            {
                exhabitDomainModel = await _exhabitService.UpdateExhabit(exhabitToUpdate.DomainModel);
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

            if (exhabitDomainModel.IsSuccessful == false)
            {
                return BadRequest(exhabitDomainModel.ErrorMessage);
            }

            return Accepted("exhabit//" + exhabitDomainModel.DomainModel.Id, exhabitDomainModel.DomainModel);

        }

        //[Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ResponseModel<ExhabitDomainModel>>> Delete(Guid id)
        {
            ResponseModel<ExhabitDomainModel> deletedExhabit;
            try
            {
                deletedExhabit = await _exhabitService.DeleteExhabit(id);
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

            if (deletedExhabit == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.EXHABIT_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            if (deletedExhabit.IsSuccessful == false)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel
                {
                    ErrorMessage = "Eksponat se izlaze na sledecoj izlozbi",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponseModel);
            }

            return Accepted("exhabit//" + deletedExhabit.DomainModel.Id, deletedExhabit.DomainModel);
        }

    }
}
