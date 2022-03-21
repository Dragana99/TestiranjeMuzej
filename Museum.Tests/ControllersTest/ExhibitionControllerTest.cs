using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Museum.API.Controllers;
using Museum.Data.Entities;
using Museum.Domain.Interface;
using Museum.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Tests.ControllersTest
{
    [TestClass]
    class ExhibitionControllerTest
    {
        private Mock<IExhibitionService> _mockExhibitionService;
        private ExhibitionEntity _exhibition;
        private ExhibitionDomainModel exhibitionDomainModel;
        private ExhibitionController exhibitionController;


    }
}
