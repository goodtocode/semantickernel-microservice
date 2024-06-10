using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microservice.Domain;
using Microservice.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservice.Api;

namespace $safeprojectname$
{
    [TestClass]
    public class BusinessGetByKeySteps : ICrudSteps<Business>
    {        
        private readonly AssociateDbContext _dbContext;
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        private readonly BusinessCreateSteps createSteps = new BusinessCreateSteps();

        public Guid SutKey { get; private set; }
        public Business Sut { get; private set; }
        public IList<Business> Suts { get; private set; } = new List<Business>();
        public IList<Business> RecycleBin { get; private set; }

        public BusinessGetByKeySteps()
        {
            _config = new AppConfigurationFactory().Create();
            _connectionString = _config[AppConfigurationKeys.SqlConnectionKey];
            _dbContext = new DbContextFactory(_connectionString).Create();
        }

        [TestMethod]
        public async Task Business_GetByKey_Green()
        {
            await GivenIHaveABusinessKey();
            GivenTheKeyIsTypeGuid();
            WhenTheBusinessExistsInPersistence();
            await WhenBusinessIsQueriedByKeyViaEntityFramework();
            ThenTheMatchingBusinessIsReturned();
        }

        public async Task GivenIHaveABusinessKey()
        {
            createSteps.GivenANewBusinessHasBeenCreated();
            await createSteps.WhenBusinessIsInsertedViaEntityFramework();
            Sut = await _dbContext.Business.FirstAsync();
            SutKey = Sut.BusinessKey;
        }
        
        public void GivenTheKeyIsTypeGuid()
        {
            Assert.IsTrue(SutKey != Guid.Empty);
        }        
        
        public void WhenTheBusinessExistsInPersistence()
        {
            Assert.IsTrue(SutKey != Guid.Empty);
        }
        
        public async Task WhenBusinessIsQueriedByKeyViaEntityFramework()
        {
            Sut = await _dbContext.Business.FirstAsync(x => x.BusinessKey == SutKey);
            Assert.IsTrue(Sut.BusinessKey != Guid.Empty);
        }
        
        public void ThenTheMatchingBusinessIsReturned()
        {
            Assert.IsTrue(Sut.BusinessKey == SutKey);
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
