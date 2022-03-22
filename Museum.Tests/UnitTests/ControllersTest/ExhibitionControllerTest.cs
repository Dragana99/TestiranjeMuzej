using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Museum.API.Controllers;
using Museum.API.Models;
using Museum.Data.Entities;
using Museum.Domain.Common;
using Museum.Domain.Interface;
using Museum.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Museum.Tests.ControllersTest
{
    [TestClass]
    public class ExhibitionControllerTest
    {
        private Mock<IExhibitionService> _mockExhibitionService;
        private ExhibitionDomainModel _exhibitionDomainModel;
        private ExhibitController exhibitionController;

        [TestInitialize]
        public void TestInit()
        {
            _exhibitionDomainModel = new ExhibitionDomainModel
            {
                Id = Guid.NewGuid(),
                Name = "Muzej Becej",
                Opening = DateTime.Now.AddDays(1),
                Description = "Opis",
                Image = "image",
                AuditoriumId = new int(),
                ExhabitId = Guid.NewGuid()
            };
            _mockExhibitionService = new Mock<IExhibitionService>();
            exhibitionController = new ExhibitController(_mockExhibitionService.Object);
        }


        [TestMethod]
        public void ExhibitionController_GetAsync_ReturnExhibition()
        {
            //Arrange
            exhibitionController = new ExhibitController(_mockExhibitionService.Object);
            var expectedResoultCount = 1;
            var expectedStatusCode = 200;
            var exhibitionsList = new List<ExhibitionDomainModel>() { _exhibitionDomainModel };
            Task<IEnumerable<ExhibitionDomainModel>> exhibitionsCollection = Task.FromResult((IEnumerable<ExhibitionDomainModel>)exhibitionsList);

            //Act
            _mockExhibitionService.Setup(x => x.GetAllAsync()).Returns(exhibitionsCollection);
            var resultAction = exhibitionController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)resultAction).Value;
            var exhibitionDomainModelsResult = (List<ExhibitionDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(exhibitionDomainModelsResult);
            Assert.AreEqual(expectedResoultCount, exhibitionDomainModelsResult.Count);
            Assert.AreEqual(_exhibitionDomainModel.Id, exhibitionDomainModelsResult[0].Id);
            Assert.AreEqual(_exhibitionDomainModel.Name, exhibitionDomainModelsResult[0].Name);
            Assert.IsInstanceOfType(exhibitionDomainModelsResult[0], typeof(ExhibitionDomainModel));
            Assert.IsInstanceOfType(exhibitionDomainModelsResult, typeof(List<ExhibitionDomainModel>));
            Assert.IsInstanceOfType(resultAction, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void EhibitionController_GetAsync_ReturnEmptyList()
        {
            //Arrange
            exhibitionController = new ExhibitController(_mockExhibitionService.Object);
            var expectedResoultCount = 0;
            var expectedStatusCode = 200;
            List<MuseumDomainModel> exhibitionList = null;
            Task<IEnumerable<ExhibitionDomainModel>> exhibitionCollection = Task.FromResult((IEnumerable<ExhibitionDomainModel>)exhibitionList);

            //Act
            _mockExhibitionService.Setup(x => x.GetAllAsync()).Returns(exhibitionCollection);
            var resultAction = exhibitionController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)resultAction).Value;
            var exhibitionDomainModelsResult = (List<ExhibitionDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(exhibitionDomainModelsResult);
            Assert.AreEqual(expectedResoultCount, exhibitionDomainModelsResult.Count);
            Assert.IsInstanceOfType(exhibitionDomainModelsResult, typeof(List<ExhibitionDomainModel>));
            Assert.IsInstanceOfType(resultAction, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void ExhibitionsController_Delete_ReturnDeletedExhibition()
        {
            //Act
            ResponseModel<ExhibitionDomainModel> responseModel = new ResponseModel<ExhibitionDomainModel>
            {
                IsSuccessful = true,
                DomainModel = _exhibitionDomainModel
            };
            int expectedStatusCode = 200;

            Task<ResponseModel<ExhibitionDomainModel>> responseTask = Task.FromResult(responseModel);
            _mockExhibitionService.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(responseTask);

            //Arrange
            var result = exhibitionController.DeleteById(It.IsAny<Guid>()).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var actualStatusCode = ((OkObjectResult)result).StatusCode;
            var resultResponse = ((OkObjectResult)result).Value;
            var exhibitionDomainModelResult = (ExhibitionDomainModel)resultResponse;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(actualStatusCode, expectedStatusCode);
            Assert.AreEqual(responseModel.DomainModel.Id, exhibitionDomainModelResult.Id);
        }

        [TestMethod]
        public void ExhibitionsController_Delete_ReturnErrorResponse()
        {
            //Act
            ResponseModel<ExhibitionDomainModel> responseModel = new ResponseModel<ExhibitionDomainModel>
            {
                IsSuccessful = false,
                DomainModel = null,
                ErrorMessage = Messages.EXHIBITION_DOES_NOT_EXIST
            };
            Task<ResponseModel<ExhibitionDomainModel>> responseTask = Task.FromResult(responseModel);
            string expectedErrorMessage = Messages.EXHIBITION_DOES_NOT_EXIST;
            int expectedStatusCode = 400;
            _mockExhibitionService.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(responseTask);

            //Arrange
            var result = exhibitionController.DeleteById(It.IsAny<Guid>()).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultErrorResponse = ((BadRequestObjectResult)result).Value;
            var actualStatusCode = ((BadRequestObjectResult)result).StatusCode;
            var exhibitionErrorResponseResult = (string)resultErrorResponse;

            //Assert

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedErrorMessage, exhibitionErrorResponseResult);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void ExhibitionController_Post_ReturnCreatedExhibition()
        {
            //Arrange
            int expectedStatusCode = 201;

            CreateExhibitionModel createExhibitionModel = new CreateExhibitionModel()
            {
                Name = "Muzej Becej",
                Opening = DateTime.Now.AddDays(1),
                Description = "Opis",
                Image = "image",
                AuditoriumId = new int(),
                ExhabitId = Guid.NewGuid()
            };
            CreateExhibitionResultModel createExhibitionResultModel = new CreateExhibitionResultModel
            {

                IsSuccessful = true,
                Exhibiition = new ExhibitionDomainModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Muzej Becej",
                    Opening = DateTime.Now.AddDays(1),
                    Description = "Opis",
                    Image = "image",
                    AuditoriumId = createExhibitionModel.AuditoriumId,
                    ExhabitId = createExhibitionModel.ExhabitId
                }
            };

            Task<CreateExhibitionResultModel> responseTask = Task.FromResult(createExhibitionResultModel);

            _mockExhibitionService.Setup(x => x.CreateExhibition(It.IsAny<ExhibitionDomainModel>())).Returns(responseTask);

            //Act
            var result = exhibitionController.PostAsync(createExhibitionModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var createdResult = ((CreatedResult)result).Value;
            var exhibitionDomainModel = (ExhibitionDomainModel)createdResult;

            //Assert
            Assert.IsNotNull(exhibitionDomainModel);
            Assert.AreEqual(createExhibitionModel.AuditoriumId, exhibitionDomainModel.AuditoriumId);
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            Assert.AreEqual(expectedStatusCode, ((CreatedResult)result).StatusCode);
        }
    

        [TestMethod]
        public void PostAsync_With_UnValid_OpeningDate_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Vreme otvaranja izlozbe ne moze da bude u proslosti";
            int expectedStatusCode = 400;

            CreateExhibitionModel createExhibitionModel = new CreateExhibitionModel()
            {
                Name = "Muzej Becej",
                Opening = DateTime.Now.AddDays(-1),
                Description = "Opis",
                Image = "image",
                AuditoriumId = new int(),
                ExhabitId = Guid.NewGuid()
            };

            //Act
            var result = exhibitionController.PostAsync(createExhibitionModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var createdResult = ((BadRequestObjectResult)result).Value;
            //  var errorResponse = ((SerializableError)createdResult).GetValueOrDefault(nameof(createProjectionModel.ProjectionTime));
            var message = (ErrorResponseModel)createdResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, message.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public void ExhibitionController_Put_ReturnUpdatedExhibition()
        {
            //Act
            ResponseModel<ExhibitionDomainModel> responseModel = new ResponseModel<ExhibitionDomainModel>
            {
                IsSuccessful = true,
                DomainModel = _exhibitionDomainModel
            };

            ResponseModel<ExhibitionDomainModel> updatedResponseModel = new ResponseModel<ExhibitionDomainModel>
            {
                IsSuccessful = true,
                DomainModel = new ExhibitionDomainModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Muzej Becej",
                    Opening = DateTime.Now.AddDays(1),
                    Description = "Opis",
                    Image = "image",
                    AuditoriumId = new int(),
                    ExhabitId = Guid.NewGuid()
                }
            };

            Task<ResponseModel<ExhibitionDomainModel>> taskResponse = Task.FromResult(responseModel);
            Task<ResponseModel<ExhibitionDomainModel>> taskResponse2 = Task.FromResult(updatedResponseModel);

            _mockExhibitionService.Setup(x => x.GetExhibitionById(It.IsAny<Guid>())).Returns(taskResponse);
            _mockExhibitionService.Setup(x => x.Update(It.IsAny<ExhibitionDomainModel>())).Returns(taskResponse2);
            int expectedStatusCode = 200;

            //Arrange
            var resultt = exhibitionController.Update(It.IsAny<Guid>(), new ExhibitionDomainModel() {
                Name = "Muzej Becej",
                Opening = DateTime.Now.AddDays(1),
                Description = "Opis",
                Image = "image",
                AuditoriumId = new int(),
                ExhabitId = Guid.NewGuid()
            }).ConfigureAwait(false).GetAwaiter().GetResult().Result;

            var resultObject = ((OkObjectResult)resultt).Value;
            var actualStatusCode = ((OkObjectResult)resultt).StatusCode;
            var resultDomainModel = ((ExhibitionDomainModel)resultObject);

            //Assert
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(resultDomainModel.Id, updatedResponseModel.DomainModel.Id);
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsInstanceOfType(resultt, typeof(OkObjectResult));
        }

        [TestMethod]
        public void ExhibitionController_Put_Returns_FailedUpdate()
        {
            ResponseModel<ExhibitionDomainModel> responseModel = new ResponseModel<ExhibitionDomainModel>
            {
                IsSuccessful = true,
                DomainModel = _exhibitionDomainModel
            };

            ResponseModel<ExhibitionDomainModel> updatedResponseModel = new ResponseModel<ExhibitionDomainModel>
            {
                IsSuccessful = false,
                DomainModel = null,
                ErrorMessage = "Failed to update exhibition!"

            };

            Task<ResponseModel<ExhibitionDomainModel>> taskResponse = Task.FromResult(responseModel);
            Task<ResponseModel<ExhibitionDomainModel>> taskResponse2 = Task.FromResult(updatedResponseModel);
            _mockExhibitionService.Setup(x => x.GetExhibitionById(It.IsAny<Guid>())).Returns(taskResponse);

            _mockExhibitionService.Setup(x => x.Update(It.IsAny<ExhibitionDomainModel>())).Returns(taskResponse2);
            int expectedStatusCode = 400;

            //Arrange
            var result = exhibitionController.Update(It.IsAny<Guid>(), new ExhibitionDomainModel()
            {
                Name = "Muzej Becej",
                Opening = DateTime.Now.AddDays(1),
                Description = "Opis",
                Image = "image",
                AuditoriumId = new int(),
                ExhabitId = Guid.NewGuid()
            }).ConfigureAwait(false).GetAwaiter().GetResult().Result;

            var resultObject = ((BadRequestObjectResult)result).Value;
            var actualStatusCode = ((BadRequestObjectResult)result).StatusCode;
            var resultString = ((ErrorResponseModel)resultObject);

            //Assert
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(resultString.ErrorMessage, updatedResponseModel.ErrorMessage);
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
