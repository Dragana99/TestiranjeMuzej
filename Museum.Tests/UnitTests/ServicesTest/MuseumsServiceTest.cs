using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Museum.Data.Entities;
using Museum.Domain.Common;
using Museum.Domain.Interface;
using Museum.Domain.Models;
using Museum.Domain.Service;
using Museum.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Museum.Tests.ServicesTest
{
    [TestClass]
    public class MuseumsServiceTest
    {
        private Mock<IMuseumsRepository> _mockMuseumsRepository;
        private Mock<IAuditoriumService> _mockAuditoriumService;
        private MuseumEntity _museum;
        private AuditoriumEntity _auditorium;
        private MuseumDomainModel _museumDomainModel;
        private AuditoriumDomainModel _auditoriumDomainModel;
        private CreateAuditoriumResultModel _createAuditoriumResultModel;
        private MuseumService museumService;

        [TestInitialize]
        public void TestInitialize()
        {
            _museum = new MuseumEntity()
            {
                Id = 1,
                Name = "Muzej Becej",
                Auditoriums = new List<AuditoriumEntity>() { }
            };

            _museumDomainModel = new MuseumDomainModel()
            {
                Id = _museum.Id,
                Name = "Muzej Becej"
            };

            _auditorium = new AuditoriumEntity()
            {
                Id = 1,
                Name = "Muzej Becej",
                MuseumId = _museum.Id,
                Museum = _museum
            };

            _auditoriumDomainModel = new AuditoriumDomainModel()
            {
                Id = _auditorium.Id,
                MuseumId = _auditorium.MuseumId,
                Name = _auditorium.Name,
            };

            _museum.Auditoriums.Add(_auditorium);

           
            _createAuditoriumResultModel = new CreateAuditoriumResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Auditorium = _auditoriumDomainModel
            };

           

            _mockMuseumsRepository = new Mock<IMuseumsRepository>();
            _mockAuditoriumService = new Mock<IAuditoriumService>();
        }


        [TestMethod]
        public void MuseumService_GetAllAsync_ReturnMuseum()
        {
            //Arrange
            museumService = new MuseumService(_mockMuseumsRepository.Object, _mockAuditoriumService.Object);
            var expectedResultCount = 1;
            var museumsList = new List<MuseumEntity>() { _museum };
            Task<IEnumerable<MuseumEntity>> museumCollection = Task.FromResult((IEnumerable<MuseumEntity>)museumsList);

            //Act
            _mockMuseumsRepository.Setup(x => x.GetAll()).Returns(museumCollection);
            var resultAction = museumService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = (List<MuseumDomainModel>)resultAction;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResultCount, result.Count);
            Assert.AreEqual(_museum.Id, result[0].Id);
            Assert.AreEqual(_museum.Name, result[0].Name);
            Assert.IsInstanceOfType(result, typeof(List<MuseumDomainModel>));
        }

        [TestMethod]
        public void MuseumService_GetAllAsync_ReturnEmptyList()
        {
            //Arrange
            museumService = new MuseumService(_mockMuseumsRepository.Object, _mockAuditoriumService.Object);
            var expectedResultCount = 0;
            var museumsList = new List<MuseumEntity>() { };
            Task<IEnumerable<MuseumEntity>> museumCollection = Task.FromResult((IEnumerable<MuseumEntity>)museumsList);

            //Act
            _mockMuseumsRepository.Setup(x => x.GetAll()).Returns(museumCollection);
            var resultAction = museumService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = (List<MuseumDomainModel>)resultAction;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResultCount, result.Count);
            Assert.IsInstanceOfType(result, typeof(List<MuseumDomainModel>));
        }

        [TestMethod]
        public void MuseumService_GetAllAsync_ReturnNull()
        {
            //Arrange
            museumService = new MuseumService(_mockMuseumsRepository.Object, _mockAuditoriumService.Object);
            List<MuseumEntity> museumList = null;
            Task<IEnumerable<MuseumEntity>> museumsCollection = Task.FromResult((IEnumerable<MuseumEntity>)museumList);

            //Act
            _mockMuseumsRepository.Setup(x => x.GetAll()).Returns(museumsCollection);
            var resultAction = museumService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }


        [TestMethod]
        public void MusuemService_GetMuseumByIdAsync_ReturnMuseum()
        {
            //Arrange
            museumService = new MuseumService(_mockMuseumsRepository.Object, _mockAuditoriumService.Object);
            Task<MuseumEntity> museum = Task.FromResult((MuseumEntity)_museum);

            //Act
            _mockMuseumsRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(museum);
            var result = museumService.GetMuseumByIdAsync(It.IsAny<int>()).ConfigureAwait(false).GetAwaiter().GetResult();
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_museum.Id, result.Id);
            Assert.AreEqual(_museum.Name, result.Name);
            Assert.IsInstanceOfType(result, typeof(MuseumDomainModel));
        }


        [TestMethod]
        public void MusuemService_GetMuseumByIdAsync_ReturnNull()
        {
            //Arrange
            museumService = new MuseumService(_mockMuseumsRepository.Object, _mockAuditoriumService.Object);
            Task<MuseumEntity> museum = Task.FromResult((MuseumEntity)null);

            //Act
            _mockMuseumsRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(museum);
            var result = museumService.GetMuseumByIdAsync(It.IsAny<int>()).ConfigureAwait(false).GetAwaiter().GetResult();
           
            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void MusuemService_DeleteMuseum_ReturnsDeletedMuseum()
        {
            //Arrange
            museumService = new MuseumService(_mockMuseumsRepository.Object, _mockAuditoriumService.Object);

            //Act
            _mockMuseumsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_museum);
            _mockMuseumsRepository.Setup(x => x.Save());
            var result = museumService.DeleteMuseum(It.IsAny<int>()).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_museum.Id, result.Id);
            Assert.AreEqual(_museum.Name, result.Name);
            Assert.IsInstanceOfType(result, typeof(MuseumDomainModel));
        }

        [TestMethod]
        public void MusuemService_DeleteMuseum_ReturnNull()
        {
            //Arrange
            museumService = new MuseumService(_mockMuseumsRepository.Object, _mockAuditoriumService.Object);
            MuseumEntity museum = null;

            //Act
            _mockMuseumsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(museum);
            _mockMuseumsRepository.Setup(x => x.Save());
            var result = museumService.DeleteMuseum(It.IsAny<int>()).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void MusuemService_AddMuseum_ReturnNewMuseum()
        {
            //Arrange
            museumService = new MuseumService(_mockMuseumsRepository.Object, _mockAuditoriumService.Object);
            Task<MuseumEntity> museum = Task.FromResult((MuseumEntity)null);

            //Act
            _mockMuseumsRepository.Setup(x => x.GetByMuseumName(It.IsAny<string>())).Returns(museum);
            _mockMuseumsRepository.Setup(x => x.Insert(It.IsAny<MuseumEntity>())).Returns(_museum);
            _mockMuseumsRepository.Setup(x => x.Save());
            var result = museumService.AddMuseum(_museumDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsNull(result.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(CreateMuseumResultModel));

        }

        [TestMethod]
        public void MusuemService_AddMuseum_MuseumWithSameNameAlreadyExists_ReturnErrorMessage()
        {
            //Arrange
            var expectedMessage = Messages.MUSEUM_SAME_NAME;
            museumService = new MuseumService(_mockMuseumsRepository.Object, _mockAuditoriumService.Object);
            Task<MuseumEntity> museum = Task.FromResult((MuseumEntity)_museum);

            //Act
            _mockMuseumsRepository.Setup(x => x.GetByMuseumName(It.IsAny<string>())).Returns(museum);
            _mockMuseumsRepository.Setup(x => x.Insert(It.IsAny<MuseumEntity>())).Returns(_museum);
            _mockMuseumsRepository.Setup(x => x.Save());
            var result = museumService.AddMuseum(_museumDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(expectedMessage, result.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(CreateMuseumResultModel));

        }

        [TestMethod]
        public void MusuemService_AddMuseum_ReturnMuseumCreationError()
        {
            //Arrange
            var expectedMessage = Messages.MUSEUM_CREATION_ERROR;
            museumService = new MuseumService(_mockMuseumsRepository.Object, _mockAuditoriumService.Object);
            Task<MuseumEntity> museum = Task.FromResult((MuseumEntity)null);
            MuseumEntity insertMuseum = null;

            //Act
            _mockMuseumsRepository.Setup(x => x.GetByMuseumName(It.IsAny<string>())).Returns(museum);
            _mockMuseumsRepository.Setup(x => x.Insert(It.IsAny<MuseumEntity>())).Returns(insertMuseum);
            _mockMuseumsRepository.Setup(x => x.Save());
            var result = museumService.AddMuseum(_museumDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual(expectedMessage, result.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(CreateMuseumResultModel));

        }
    
        [TestMethod]
        public void MusuemService_UpdateMuseum_ReturnsUpdatedMuseum()
        {
            //Arrange
            museumService = new MuseumService(_mockMuseumsRepository.Object, _mockAuditoriumService.Object);

            //Act
            _mockMuseumsRepository.Setup(x => x.Update(It.IsAny<MuseumEntity>())).Returns(_museum);
            _mockMuseumsRepository.Setup(x => x.Save());
            var result = museumService.UpdateMuseum(_museumDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_museum.Id, result.Id);
            Assert.AreEqual(_museum.Name, result.Name);
            Assert.IsInstanceOfType(result, typeof(MuseumDomainModel));
        }

        [TestMethod]
        public void MusuemService_UpdateMuseum_ReturnNull()
        {
            //Arrange
            museumService = new MuseumService(_mockMuseumsRepository.Object, _mockAuditoriumService.Object);
           MuseumEntity museum = null;

            //Act
            _mockMuseumsRepository.Setup(x => x.Update(It.IsAny<MuseumEntity>())).Returns(museum);
            _mockMuseumsRepository.Setup(x => x.Save());
            var result = museumService.UpdateMuseum(_museumDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }
    }
}
