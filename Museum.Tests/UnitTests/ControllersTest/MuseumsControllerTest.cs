using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class MuseumsControllerTest
    {
        private Mock<IMuseumService> _mockMuseumService;
        private MuseumEntity _museum;
        private MuseumDomainModel _museumDomainModel;
        private MuseumsController museumsController;

        [TestInitialize]
        public void TestInitialize()
        {
            _museum = new MuseumEntity()
            {
                Id = 1,
                Name = "Muzej Becej",
                City = "Becej",
                Address = "Glavna 88",
                Email = "muzbecej@gmail.com",
                Phone = "+344567456"
            };

            _museumDomainModel = new MuseumDomainModel()
            {
                Id = _museum.Id,
                Name = "Muzej Becej",
                City = "Becej",
                Address = "Glavna 88",
                Email = "muzbecej@gmail.com",
                Phone = "+344567456"
            };

            _mockMuseumService = new Mock<IMuseumService>();
        }


        [TestMethod]
        public void MuseumController_GetAsync_ReturnMuseums()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedResoultCount = 1;
            var expectedStatusCode = 200;
            var museumsList = new List<MuseumDomainModel>() { _museumDomainModel };
            Task<IEnumerable<MuseumDomainModel>> museumsCollection = Task.FromResult((IEnumerable<MuseumDomainModel>)museumsList);

            //Act
            _mockMuseumService.Setup(x => x.GetAllAsync()).Returns(museumsCollection);
            var resultAction = museumsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)resultAction).Value;
            var musuemDomainModelsResult = (List<MuseumDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(musuemDomainModelsResult);
            Assert.AreEqual(expectedResoultCount, musuemDomainModelsResult.Count);
            Assert.AreEqual(_museumDomainModel.Id, musuemDomainModelsResult[0].Id);
            Assert.AreEqual(_museumDomainModel.Name, musuemDomainModelsResult[0].Name);
            Assert.IsInstanceOfType(musuemDomainModelsResult[0], typeof(MuseumDomainModel));
            Assert.IsInstanceOfType(musuemDomainModelsResult, typeof(List<MuseumDomainModel>));
            Assert.IsInstanceOfType(resultAction, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void MuseumsController_GetAsync_ReturnEmptyList()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedResoultCount = 0;
            var expectedStatusCode = 200;
            List<MuseumDomainModel> museumsList = null;
            Task<IEnumerable<MuseumDomainModel>> museumsCollection = Task.FromResult((IEnumerable<MuseumDomainModel>)museumsList);

            //Act
            _mockMuseumService.Setup(x => x.GetAllAsync()).Returns(museumsCollection);
            var resultAction = museumsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)resultAction).Value;
            var cinemasDomainModelsResult = (List<MuseumDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(cinemasDomainModelsResult);
            Assert.AreEqual(expectedResoultCount, cinemasDomainModelsResult.Count);
            Assert.IsInstanceOfType(cinemasDomainModelsResult, typeof(List<MuseumDomainModel>));
            Assert.IsInstanceOfType(resultAction, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void MuseumsController_GetByIdAsync_ReturnMuseum()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedStatusCode = 200;
            Task<MuseumDomainModel> cinema = Task.FromResult((MuseumDomainModel)_museumDomainModel);

            //Act
            _mockMuseumService.Setup(x => x.GetMuseumByIdAsync(It.IsAny<int>())).Returns(cinema);
            var resultAction = museumsController.GetByIdAsync(It.IsAny<int>()).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)resultAction).Value;
            var museumsDomainModelResult = (MuseumDomainModel)resultList;

            //Assert
            Assert.IsNotNull(museumsDomainModelResult);
            Assert.AreEqual(_museumDomainModel.Id, museumsDomainModelResult.Id);
            Assert.AreEqual(_museumDomainModel.Name, museumsDomainModelResult.Name);
            Assert.IsInstanceOfType(museumsDomainModelResult, typeof(MuseumDomainModel));
            Assert.IsInstanceOfType(resultAction, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void MuseumsController_GetByIdAsync_ReturnErrorMessage()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedStatusCode = 404;
            var expectedMessage = Messages.MUSEUM_DOES_NOT_EXIST;
            Task<MuseumDomainModel> museum = Task.FromResult((MuseumDomainModel)null);

            //Act
            _mockMuseumService.Setup(x => x.GetMuseumByIdAsync(It.IsAny<int>())).Returns(museum);
            var resultAction = museumsController.GetByIdAsync(It.IsAny<int>()).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var result = ((NotFoundObjectResult)resultAction).Value;
            var errorMessage = (string)result;

            //Assert
            Assert.AreEqual(expectedMessage, errorMessage);
            Assert.IsInstanceOfType(resultAction, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void MuseumsController_Delete_ReturnDeletedMuseum()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedStatusCode = 202;
            Task<MuseumDomainModel> museum = Task.FromResult((MuseumDomainModel)_museumDomainModel);

            //Act
            _mockMuseumService.Setup(x => x.DeleteMuseum(It.IsAny<int>())).Returns(museum);
            var resultAction = museumsController.Delete(It.IsAny<int>()).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = ((AcceptedResult)resultAction).Value;
            var model = (MuseumDomainModel)result;

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_museumDomainModel.Id, model.Id);
            Assert.AreEqual(_museumDomainModel.Name, model.Name);
            Assert.IsInstanceOfType(resultAction, typeof(AcceptedResult));
            Assert.AreEqual(expectedStatusCode, ((AcceptedResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void MuseumsController_Delete_ReturnErrorResponse()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedStatusCode = 500;
            var expectedMessage = Messages.MUSEUM_DOES_NOT_EXIST;
            Task<MuseumDomainModel> museum = Task.FromResult((MuseumDomainModel)null);

            //Act
            _mockMuseumService.Setup(x => x.DeleteMuseum(It.IsAny<int>())).Returns(museum);
            var resultAction = museumsController.Delete(It.IsAny<int>()).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = ((ObjectResult)resultAction).Value;
            var errorMessage = (ErrorResponseModel)result;
            //Assert
            Assert.AreEqual(expectedMessage, errorMessage.ErrorMessage);
            Assert.IsInstanceOfType(resultAction, typeof(ObjectResult));
            Assert.AreEqual(expectedStatusCode, (int)errorMessage.StatusCode);
        }

        [TestMethod]
        public void MuseumsController_Delete_ReturnDbUpdateException()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedStatusCode = 400;
            var expectedMessage = Messages.MUSEUM_DOES_NOT_EXIST;
            Task<MuseumDomainModel> cinema = Task.FromResult((MuseumDomainModel)null);
            Exception exception = new Exception(Messages.MUSEUM_DOES_NOT_EXIST);
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            //Act
            _mockMuseumService.Setup(x => x.DeleteMuseum(It.IsAny<int>())).Throws(dbUpdateException);
            var resultAction = museumsController.Delete(It.IsAny<int>()).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = ((BadRequestObjectResult)resultAction).Value;
            var errorMessage = (ErrorResponseModel)result;
            //Assert
            Assert.AreEqual(expectedMessage, errorMessage.ErrorMessage);
            Assert.IsInstanceOfType(resultAction, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, (int)errorMessage.StatusCode);
        }

        [TestMethod]
        public void MuseumsController_Delete_ReturnArgumentNullException()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedStatusCode = 400;
            var expectedMessage = Messages.MUSEUM_DOES_NOT_EXIST;
            Task<MuseumDomainModel> cinema = Task.FromResult((MuseumDomainModel)null);
            Exception exception = new Exception(Messages.MUSEUM_DOES_NOT_EXIST);
            ArgumentNullException dbUpdateException = new ArgumentNullException("Error.", exception);

            //Act
            _mockMuseumService.Setup(x => x.DeleteMuseum(It.IsAny<int>())).Throws(dbUpdateException);
            var resultAction = museumsController.Delete(It.IsAny<int>()).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = ((BadRequestObjectResult)resultAction).Value;
            var errorMessage = (ErrorResponseModel)result;
            //Assert
            Assert.AreEqual(expectedMessage, errorMessage.ErrorMessage);
            Assert.IsInstanceOfType(resultAction, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, (int)errorMessage.StatusCode);
        }

        [TestMethod]
        public void MuseumsController_Post_ReturnModelStateError()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedStatusCode = 400;
            museumsController.ModelState.AddModelError("test", "test");

            //Act
            var resultAction = museumsController.Post(new CreateMuseumModel() { Name = "Muzej Test" }).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = ((BadRequestObjectResult)resultAction).Value;

            //Assert
            Assert.IsInstanceOfType(resultAction, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void MuseumsController_Post_ReturnCreatedMuseum()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedStatusCode = 201;
            var museumModel = new CreateMuseumModel()
            {
                Name = "Muzej Becej",
                City = "Becej",
                Address = "Glavna 88",
                Email = "muzbecej@gmail.com",
                Phone = "+344567456"
            };

            var responseModel = new CreateMuseumResultModel()
            {
                ErrorMessage = null,
                IsSuccessful = true,
                Museum = _museumDomainModel
            };
            Task<CreateMuseumResultModel> model = Task.FromResult(responseModel);

            //Act
            _mockMuseumService.Setup(x => x.AddMuseum(It.IsAny<MuseumDomainModel>())).Returns(model);
            var resultAction = museumsController.Post(museumModel).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = ((CreatedResult)resultAction).Value;
            var createdMuseumModel = (MuseumDomainModel)result;

            //Assert
            Assert.IsInstanceOfType(resultAction, typeof(CreatedResult));
            Assert.AreEqual(_museumDomainModel.Id, createdMuseumModel.Id);
            Assert.AreEqual(_museumDomainModel.Name, createdMuseumModel.Name);
            Assert.AreEqual(expectedStatusCode, ((CreatedResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void MuseumsController_Post_ReturnErrorResponseModel()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedStatusCode = 400;
            var museumModel = new CreateMuseumModel()
            {
                Name = "Muzej Becej",
                City = "Becej",
                Address = "Glavna 88",
                Email = "muzbecej@gmail.com",
                Phone = "+344567456"
            };

            var responseModel = new CreateMuseumResultModel()
            {
                ErrorMessage = Messages.MUSEUM_CREATION_ERROR,
                IsSuccessful = false,
                Museum = _museumDomainModel
            };
            Task<CreateMuseumResultModel> model = Task.FromResult(responseModel);

            //Act
            _mockMuseumService.Setup(x => x.AddMuseum(It.IsAny<MuseumDomainModel>())).Returns(model);
            var resultAction = museumsController.Post(museumModel).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = ((BadRequestObjectResult)resultAction).Value;
            var errorMessage = (ErrorResponseModel)result;

            //Assert
            Assert.IsInstanceOfType(resultAction, typeof(BadRequestObjectResult));
            Assert.IsNotNull(errorMessage.ErrorMessage);
            Assert.AreEqual(Messages.MUSEUM_CREATION_ERROR, errorMessage.ErrorMessage);
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void MuseumsController_Put_ReturnModelStateError()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedStatusCode = 400;
            museumsController.ModelState.AddModelError("test", "test");

            //Act
            var resultAction = museumsController.Put(It.IsAny<int>(), new CreateMuseumModel() { Name = "Muzej" }).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = ((BadRequestObjectResult)resultAction).Value;

            //Assert
            Assert.IsInstanceOfType(resultAction, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void MuseumsController_Put_ReturnUpdatedMuseum()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedStatusCode = 202;
            Task<MuseumDomainModel> cinema = Task.FromResult((MuseumDomainModel)_museumDomainModel);
            var updatedMuseum = new MuseumDomainModel()
            {
                Id = _museumDomainModel.Id,
                Name = "Novi Muzej",
                City = "Becej",
                Address = "Glavna 88",
                Email = "muzbecej@gmail.com",
                Phone = "+344567456"
            };
            Task<MuseumDomainModel> updated = Task.FromResult((MuseumDomainModel)updatedMuseum);

            //Act
            _mockMuseumService.Setup(x => x.GetMuseumByIdAsync(It.IsAny<int>())).Returns(cinema);
            _mockMuseumService.Setup(x => x.UpdateMuseum(It.IsAny<MuseumDomainModel>())).Returns(updated);
            var resultAction = museumsController.Put(It.IsAny<int>(), new CreateMuseumModel() { Name = "Novi Muzej" }).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = ((AcceptedResult)resultAction).Value;
            var MuseumDomainModelResult = (MuseumDomainModel)result;

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(updatedMuseum.Id, MuseumDomainModelResult.Id);
            Assert.AreEqual(updatedMuseum.Name, MuseumDomainModelResult.Name);
            Assert.IsInstanceOfType(resultAction, typeof(AcceptedResult));
            Assert.AreEqual(expectedStatusCode, ((AcceptedResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void museumsController_Put_ReturnMuseumDoesntExistsError()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedStatusCode = 400;
            var expectedMessage = Messages.MUSEUM_DOES_NOT_EXIST;
            Task<MuseumDomainModel> museum = Task.FromResult((MuseumDomainModel)null);

            //Act
            _mockMuseumService.Setup(x => x.GetMuseumByIdAsync(It.IsAny<int>())).Returns(museum);
            var resultAction = museumsController.Put(It.IsAny<int>(), new CreateMuseumModel() { Name = "Novi Muzej" }).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = ((BadRequestObjectResult)resultAction).Value;
            var errorResponseModel = (ErrorResponseModel)result;

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedMessage, errorResponseModel.ErrorMessage);
            Assert.IsInstanceOfType(resultAction, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)resultAction).StatusCode);
        }

        [TestMethod]
        public void museumsController_Put_ReturnUpdateDbException()
        {
            //Arrange
            museumsController = new MuseumsController(_mockMuseumService.Object);
            var expectedStatusCode = 400;
            Task<MuseumDomainModel> cinema = Task.FromResult((MuseumDomainModel)_museumDomainModel);
            Exception exception = new Exception(Messages.MUSEUM_DOES_NOT_EXIST);
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            _mockMuseumService.Setup(x => x.UpdateMuseum(It.IsAny<MuseumDomainModel>())).Throws(dbUpdateException);
            var resultAction = museumsController.Put(It.IsAny<int>(), new CreateMuseumModel() { Name = "Novi Muzej" }).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = ((BadRequestObjectResult)resultAction).Value;
            var errorMessage = (ErrorResponseModel)result;
            //Assert
            Assert.AreEqual(dbUpdateException.InnerException.Message.ToString(), errorMessage.ErrorMessage);
            Assert.IsInstanceOfType(resultAction, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, (int)errorMessage.StatusCode);
        }

    }
}
