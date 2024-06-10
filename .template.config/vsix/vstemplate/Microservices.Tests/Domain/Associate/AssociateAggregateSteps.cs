using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microservice.Domain;
using Microservice.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservice.Application;
using Microservice.Api;

namespace $safeprojectname$
{
    [TestClass]
    public class BusinessAggregateSteps : IAggregateSteps<AssociateAggregate>
    {
        private readonly AssociateDbContext _dbContext;
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        private int _rowsAffected;

        public Guid SutKey { get; private set; }
        public AssociateAggregate Aggregate { get; private set; }
        public IList<AssociateAggregate> RecycleBin { get; private set; }

        public Business SutBusiness { get; private set; }

        public BusinessAggregateSteps()
        {
            _config = new AppConfigurationFactory().Create();
            _connectionString = _config[AppConfigurationKeys.SqlConnectionKey];
            _dbContext = new DbContextFactory(_connectionString).Create();
            Aggregate = new AssociateAggregate(_dbContext);
        }

        [TestMethod]
        public async Task Associate_Aggregate_Green()
        {
            GivenANewBusinessIsCreatedForTheAggregate();
            GivenTheNewBusinessForTheAggregateIsSerializable();
            await WhenTheBusinessDoesNotExistInPersistence();
            await WhenTheBusinessIsSavedViaTheAggregate();
            await ThenTheAggregateInsertedBusinessCanBeQueriedByKey();
        }

        public void GivenANewBusinessIsCreatedForTheAggregate()
        {
            SutKey = Guid.Empty;
            SutBusiness = new Business()
            {
                BusinessKey = SutKey,
                BusinessName = $"{GetType().FullName}, Inc."
            };
        }

        public void GivenTheNewBusinessForTheAggregateIsSerializable()
        {
            var serialized = JsonConvert.SerializeObject(SutBusiness);
            var deserialized = JsonConvert.DeserializeObject<Business>(serialized);
            Assert.IsTrue(deserialized.BusinessName.Length > 0);
        }

        public async Task WhenTheBusinessDoesNotExistInPersistence()
        {
            var found = await _dbContext.Business.Where(x => x.BusinessKey == SutKey).AnyAsync();
            Assert.IsFalse(found);
        }

        public async Task WhenTheBusinessIsSavedViaTheAggregate()
        {
            _rowsAffected = await Aggregate.BusinessSaveAsync(SutBusiness);
            SutKey = SutBusiness.BusinessKey;
            Assert.IsTrue(_rowsAffected > 0);
        }

        public async Task ThenTheAggregateInsertedBusinessCanBeQueriedByKey()
        {
            var found = await _dbContext.Business.Where(x => x.BusinessKey == SutKey).AnyAsync();
            Assert.IsTrue(found);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await Aggregate.BusinessDeleteAsync(SutBusiness.RowKey);
        }
    }
}
