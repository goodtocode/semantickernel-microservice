using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class BusinessesGetSteps : ICrudSteps<Business>
    {
        private readonly AssociateDbContext _dbContext;
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        private readonly BusinessCreateSteps createSteps = new BusinessCreateSteps();

        public Business Sut { get; private set; }
        public IList<Business> Suts { get; private set; } = new List<Business>();
        public Guid SutKey { get; private set; }
        public IList<Business> RecycleBin { get; set; }

        public BusinessesGetSteps()
        {
            _config = new AppConfigurationFactory().Create();
            _connectionString = _config[AppConfigurationKeys.SqlConnectionKey];
            _dbContext = new DbContextFactory(_connectionString).Create();
        }

        [TestMethod]
        public async Task Businesses_Get_Green()
        {
            await GivenIRequestTheListOfBusinesses();
            await WhenBusinessesAreQueriedViaEntityFrameworkAsync();
            ThenAllPersistedBusinessesAreReturned();
        }

        public async Task GivenIRequestTheListOfBusinesses()
        {
            createSteps.GivenANewBusinessHasBeenCreated();
            await createSteps.WhenBusinessIsInsertedViaEntityFramework();
            Sut = await _dbContext.Business.FirstAsync();
            SutKey = Sut.BusinessKey;
        }

        public async Task WhenBusinessesAreQueriedViaEntityFrameworkAsync()
        {
            Suts = await _dbContext.Business.Take(10).ToListAsync();
            Sut = Suts.FirstOrDefault();
            SutKey = Sut.BusinessKey;

        }

        public void ThenAllPersistedBusinessesAreReturned()
        {
            Assert.IsTrue(Sut.BusinessKey != Guid.Empty);
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
