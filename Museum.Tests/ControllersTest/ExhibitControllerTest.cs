using System;
using System.Collections.Generic;
using System.Text;
using Museum.API.Models;
using Museum.Domain.Common;
using Museum.Domain.Interface;
using Museum.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Museum.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Museum.Tests.ControllersTest
{
    [TestClass]
    public class ExhibitControllerTest
    {
        private Mock<IExhabitService> _mockExhibitService;
        private ExhabitDomainModel _exhibitDomainModel;
        private ExhabitController exhibitController;

        [TestInitialize]
        public void TestInit()
        {
            _exhibitDomainModel = new ExhabitDomainModel
            {
                Id = Guid.NewGuid(),
                Name = "Eksponat",
                Picture = "url",
                Year = 1988
            };
            _mockExhibitService = new Mock<IExhabitService>();
            exhibitController = new ExhabitController(_mockExhibitService.Object);
        }


        [TestMethod]
        public void ExhibitController_GetAsync_ReturnExhibition()
        {
            //Arrange
            exhibitController = new ExhabitController(_mockExhibitService.Object);
            var expectedResoultCount = 1;
            var expectedStatusCode = 200;
            var exhibitList = new List<ExhabitDomainModel>() { _exhibitDomainModel };
            Task<IEnumerable<ExhabitDomainModel>> exhibitCollection = Task.FromResult((IEnumerable<ExhabitDomainModel>)exhibitList);

            //Act
            _mockExhibitService.Setup(x => x.GetAllExhabits()).Returns(exhibitCollection);
            var resultAction = exhibitController.GetAll().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)resultAction).Value;

            //Assert
            Assert.IsInstanceOfType(resultAction, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void EhibitionController_GetAsync_ReturnEmptyList()
        {
            //Arrange
            exhibitController = new ExhabitController(_mockExhibitService.Object);
            var expectedResoultCount = 0;
            var expectedStatusCode = 200;
            List<MuseumDomainModel> exhibitList = null;
            Task<IEnumerable<ExhabitDomainModel>> exhibitionCollection = Task.FromResult((IEnumerable<ExhabitDomainModel>)exhibitList);

            //Act
            _mockExhibitService.Setup(x => x.GetAllExhabits()).Returns(exhibitionCollection);
            var resultAction = exhibitController.GetAll().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)resultAction).Value;
            var exhibitDomainModelsResult = (List<ExhabitDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(exhibitDomainModelsResult);
            Assert.AreEqual(expectedResoultCount, exhibitDomainModelsResult.Count);
            Assert.IsInstanceOfType(resultAction, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void ExhibitController_Delete_ReturnDeletedExhibit()
        {
            //Arrange
            int expectedStatusCode = 202;
            
            ResponseModel<ExhabitDomainModel> responseModel = new ResponseModel<ExhabitDomainModel>
            {
                DomainModel = _exhibitDomainModel,
                IsSuccessful = true
            };
            Task<ResponseModel<ExhabitDomainModel>> responseTask = Task.FromResult(responseModel);
            _mockExhibitService.Setup(x => x.DeleteExhabit(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = exhibitController.Delete(It.IsAny<Guid>()).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultObject = ((AcceptedResult)result).Value;
            var actualStatusCode = ((AcceptedResult)result).StatusCode;
            var resultResponseModel = (ExhabitDomainModel)resultObject;

            //Assert
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));
        }

        [TestMethod]
        public void ExhibitController_Delete_ReturnErrorResponse()
        {
            //Act
            ResponseModel<ExhabitDomainModel> responseModel = new ResponseModel<ExhabitDomainModel>
            {
                IsSuccessful = false,
                DomainModel = null,
                ErrorMessage = "Eksponat ne postoji."
            };
            Task<ResponseModel<ExhabitDomainModel>> responseTask = Task.FromResult(responseModel);
            string expectedErrorMessage = "Eksponat ne postoji.";
            int expectedStatusCode = 400;
            _mockExhibitService.Setup(x => x.DeleteExhabit(It.IsAny<Guid>())).Returns(responseTask);

            //Arrange
            var result = exhibitController.Delete(It.IsAny<Guid>()).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultObject = ((BadRequestObjectResult)result).Value;
            var errorMessage = (ErrorResponseModel)resultObject;


            //Assert

            //Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedErrorMessage, errorMessage.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void ExhibitController_Post_ReturnCreatedExhibit()
        {
            //Arrange
            int expectedStatusCode = 201;
            exhibitController = new ExhabitController(_mockExhibitService.Object);
            var exhibitModel = new CreateExhabitModel()
            {
                Name = "Eksponat",
                Picture = "url",
                Year = 1988
            };
            ResponseModel<ExhabitDomainModel> responseModel = new ResponseModel<ExhabitDomainModel>
            {
                DomainModel = _exhibitDomainModel,
                IsSuccessful = true
            };
            Task<ResponseModel<ExhabitDomainModel>> responseTask = Task.FromResult(responseModel);
            _mockExhibitService.Setup(x => x.AddExhabit(It.IsAny<ExhabitDomainModel>())).Returns(responseTask);

            //Act
            var result = exhibitController.Post(exhibitModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultObject = ((CreatedResult)result).Value;
            var actualStatusCode = ((CreatedResult)result).StatusCode;
            var resultResponseModel = (ExhabitDomainModel)resultObject;

            //Assert
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
        }


        [TestMethod]
        public void ExhibitionController_Put_ReturnUpdatedExhibition()
        {
            //Arrange
            int expectedStatusCode = 202;
            
            var updatedMovie = new ExhabitDomainModel
            {
                Id = _exhibitDomainModel.Id,
                Picture = _exhibitDomainModel.Picture,
                Year = _exhibitDomainModel.Year
            };

            ResponseModel<ExhabitDomainModel> responseModel = new ResponseModel<ExhabitDomainModel>
            {
                DomainModel = _exhibitDomainModel,
                IsSuccessful = true
            };
            ResponseModel<ExhabitDomainModel> updatedResponseModel = new ResponseModel<ExhabitDomainModel>
            {
                DomainModel = updatedMovie,
                IsSuccessful = true
            };

            Task<ResponseModel<ExhabitDomainModel>> responseTask = Task.FromResult(responseModel);
            Task<ResponseModel<ExhabitDomainModel>> updatedResponseTask = Task.FromResult(updatedResponseModel);

            _mockExhibitService.Setup(x => x.GetExhabitByIdAsync(It.IsAny<Guid>())).Returns(updatedResponseTask);
            _mockExhibitService.Setup(x => x.UpdateExhabit(It.IsAny<ExhabitDomainModel>())).Returns(responseTask);

            //Act
            var result = exhibitController.Put(It.IsAny<Guid>(), new CreateExhabitModel
            {
                Name = "Eksponat",
                Picture = "url",
                Year = 1988
            }).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultObject = ((AcceptedResult)result).Value;
            var actualStatusCode = ((AcceptedResult)result).StatusCode;
            var resultResponseModel = (ExhabitDomainModel)resultObject;

            //Assert
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));
        }

       
    }
}
