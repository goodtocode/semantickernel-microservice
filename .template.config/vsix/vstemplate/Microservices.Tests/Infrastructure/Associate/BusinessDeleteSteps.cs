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
    public class BusinessDeleteSteps : ICrudSteps<Business>
    {
        private readonly AssociateDbContext _dbContext;
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        private readonly BusinessCreateSteps createSteps = new BusinessCreateSteps();

        public Guid SutKey { get; private set; }
        public Business Sut { get; private set; }
        public IList<Business> Suts { get; private set; } = new List<Business>();
        public IList<Business> RecycleBin { get; set; } = new List<Business>();

        public BusinessDeleteSteps()
        {
            _config = new AppConfigurationFactory().Create();
            _connectionString = _config[AppConfigurationKeys.SqlConnectionKey];
            _dbContext = new DbContextFactory(_connectionString).Create();
        }

        [TestMethod]
        public async Task Business_Delete_Green()
        {
            await GivenAnBusinessHasBeenQueriedToBeDeleted();
            GivenABusinessToBeDeletedWasFoundInPersistence();
            await WhenTheBusinessIsDeletedViaEntityFramework();
            await ThenTheDeletedBusinessIsQueriedByKey();
            ThenTheBusinessIsNotFound();
        }

        public async Task GivenAnBusinessHasBeenQueriedToBeDeleted()
        {
            createSteps.GivenANewBusinessHasBeenCreated();
            await createSteps.WhenBusinessIsInsertedViaEntityFramework();
            Sut = await _dbContext.Business.FirstAsync();
            SutKey = Sut.BusinessKey;
        }

        public void GivenABusinessToBeDeletedWasFoundInPersistence()
        {
            Assert.IsTrue(Sut.BusinessKey != Guid.Empty);
        }

        public async Task WhenTheBusinessIsDeletedViaEntityFramework()
        {
            _dbContext.Entry(Sut).State = EntityState.Deleted;
            var result = await _dbContext.SaveChangesAsync();
            Assert.IsTrue(result > 0);
        }

        public async Task ThenTheDeletedBusinessIsQueriedByKey()
        {
            Sut = await _dbContext.Business.FirstOrDefaultAsync(x => x.BusinessKey == SutKey);
        }

        public void ThenTheBusinessIsNotFound()
        {
            Assert.IsTrue(Sut?.BusinessKey != SutKey);
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
