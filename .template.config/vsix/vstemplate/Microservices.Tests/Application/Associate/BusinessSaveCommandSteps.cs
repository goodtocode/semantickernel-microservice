using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microservice.Application;
using Microservice.Domain;
using Microservice.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservice.Api;

namespace $safeprojectname$
{
    [TestClass]
    public class BusinessSaveCommandSteps : ICommandSteps<Business>
    {
        private readonly AssociateDbContext _dbContext;
        private readonly string _connectionString;
        private readonly IConfiguration _config;

        public Guid SutKey { get; private set; }
        public Business Sut { get; private set; }
        public IList<Business> RecycleBin { get; private set; }

        public BusinessSaveCommandSteps()
        {
            _config = new AppConfigurationFactory().Create();
            _connectionString = _config[AppConfigurationKeys.SqlConnectionKey];
            _dbContext = new DbContextFactory(_connectionString).Create();
        }

        [TestMethod]
        public async Task Business_Save_Green()
        {
            GivenANewBusinessSaveCommandHasBeenCreated();
            GivenTheBusinessSaveCommandValidates();
            await WhenTheBusinessIsInsertedViaCQRSCommand();
            await ThenTheCQRSInsertedBusinessCanBeQueriedByKey();
        }

        public void GivenANewBusinessSaveCommandHasBeenCreated()
        {
            SutKey = Guid.Empty;
            Sut = new Business()
            {
                BusinessKey = SutKey,
                BusinessName = "BusinessSaveCommandSteps.cs Test",
                TaxNumber = string.Empty
            };
        }

        public void GivenTheBusinessSaveCommandValidates()
        {
            Assert.IsFalse(Sut.BusinessName.Length == 0);
        }
        
        public async Task WhenTheBusinessIsInsertedViaCQRSCommand()
        {
            var query = new BusinessSaveCommand(Sut);
            var handle = new BusinessSaveHandler(_dbContext);
            var response = await handle.Handle(query, new System.Threading.CancellationToken());
            SutKey = response.Result.BusinessKey;
            Assert.IsTrue(SutKey != Guid.Empty);
        }
        
        public async Task ThenTheCQRSInsertedBusinessCanBeQueriedByKey()
        {
            var found = await _dbContext.Business.Where(x => x.BusinessKey == SutKey).AnyAsync();
            Assert.IsTrue(found);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            foreach (var item in RecycleBin)
            {
                _dbContext.Entry(item).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}