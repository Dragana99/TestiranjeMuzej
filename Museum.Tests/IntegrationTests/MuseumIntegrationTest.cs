using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Museum.Data.Context;
using Museum.Data.Entities;
using Museum.Domain.Interface;
using Museum.Domain.Service;
using Museum.Repositories;
using NUnit.Framework;


namespace Museum.Tests.IntegrationTests
{
    [TestFixture]
    public class MuseumIntegrationTest
    {
        public MuseumService service;
        public MuseumContext _context;

        [SetUp]
        public void SetUp()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<MuseumContext>();

            builder.UseSqlServer($"Data Source=.;Initial Catalog=Museum;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
                    .UseInternalServiceProvider(serviceProvider);


            _context = new MuseumContext(builder.Options);
            _context.Database.Migrate();


            List<MuseumEntity> museums = new List<MuseumEntity>
            {
                new MuseumEntity{ Name = "Integracija 1", Address = "Adresa 55", City = "Grad 1", Email = "mail@gmail.com", Phone = "+381966335", Auditoriums = null},
                new MuseumEntity{ Name = "Integracija 2", Address = "Adresa 4A", City = "Grad 2", Email = "email@gmail.com", Phone = "+38160558899", Auditoriums = null}

            };

            _context.Museum.AddRange(museums);
            _context.SaveChanges();

            service = new MuseumService(new MuseumsRepository(_context));


        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
        }



        [Test]
        public void GetAllMuseums_NameOfFirstInsertedMuseumViaIntegration()
        {
            var allMuseumsIntoDb = service.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            var someDatas = allMuseumsIntoDb.ToArray();
            Assert.AreEqual(someDatas[1].Name, "Muzej Novi Sad      ");

        }

        [Test]
        public void GetAllMuseums_TypeOfMuseumId()
        {
            var allMuseumsIntoDb = service.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            var someDatas = allMuseumsIntoDb.ToArray();
            Assert.AreEqual(someDatas[1].Id.GetType().ToString(), "System.Int32");

        }



        [Test]
        public void GetAllMuseums_MuseumNameFromDatabase()
        {
            var allMuseumsIntoDb = service.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            var someDatas = allMuseumsIntoDb.ToArray();
            Assert.AreEqual(someDatas[0].Name, "Narodni Muzej Becej ");

        }

        
        [Test]
        public void GetAllMuseums_SecondInserted_MuseumName()
        {
            var allMuseumsIntoDb = service.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            var someDatas = allMuseumsIntoDb.ToArray();
            var c = someDatas.Last();
            Assert.AreEqual(c.Name, "Integracija 2");

        }


    }
}
