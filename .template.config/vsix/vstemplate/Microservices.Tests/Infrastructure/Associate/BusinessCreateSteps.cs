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
using Microservice.Api;

namespace $safeprojectname$
{
    [TestClass]
    public class BusinessCreateSteps : ICrudSteps<Business>
    {
        private readonly AssociateDbContext _dbContext;
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        private int _rowsAffected;
        
        public Guid SutKey { get; private set; }
        public Business Sut { get; private set; }
        public IList<Business> Suts { get; private set; } = new List<Business>();
        public IList<Business> RecycleBin { get; private set; } = new List<Business>();

        public BusinessCreateSteps()
        {
            _config = new AppConfigurationFactory().Create();
            _connectionString = _config[AppConfigurationKeys.SqlConnectionKey];
            _dbContext = new DbContextFactory(_connectionString).Create();
        }

        [TestMethod]
        public async Task Business_Create_Green()
        {
            GivenANewBusinessHasBeenCreated();
            GivenTheBusinessToBeCreatedViaEntityFrameworkIsSerializable();
            await WhenTheBusinessDoesNotExistInPersistenceByKey();
            await WhenBusinessIsInsertedViaEntityFramework();
            await ThenTheNewBusinessCanBeQueriedByKey();
        }

        public void GivenANewBusinessHasBeenCreated()
        {
            SutKey = Guid.Empty;
            Sut = new Business()
            {
                BusinessKey = SutKey,
                BusinessName = $"{GetType().FullName}, Inc.",
                TaxNumber = "12345"
            };
        }
        
        public void GivenTheBusinessToBeCreatedViaEntityFrameworkIsSerializable()
        {
            var comparison = Sut.BusinessName;
            var serialized = JsonConvert.SerializeObject(Sut);
            var deserialized = JsonConvert.DeserializeObject<Business>(serialized);
            Assert.IsTrue(deserialized.BusinessName == comparison);
        }

        public async Task WhenTheBusinessDoesNotExistInPersistenceByKey()
        {
            var found = await _dbContext.Business.Where(x => x.BusinessKey == SutKey).AnyAsync();
            Assert.IsFalse(found);
        }
        
        public async Task WhenBusinessIsInsertedViaEntityFramework()
        {
            _dbContext.Business.Add(Sut);
            _rowsAffected = await _dbContext.SaveChangesAsync();
            SutKey = Sut.BusinessKey;
            Assert.IsTrue(_rowsAffected > 0);
        }
        
        public async Task ThenTheNewBusinessCanBeQueriedByKey()
        {
            var found = await _dbContext.Business.Where(x => x.BusinessKey == SutKey).AnyAsync();
            Assert.IsTrue(found);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            foreach(var item in RecycleBin)
            {
                _dbContext.Entry(item).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
