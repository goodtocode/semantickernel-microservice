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
    public class BusinessUpdateSteps : ICrudSteps<Business>
    {
        private readonly AssociateDbContext _dbContext;
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        private readonly BusinessCreateSteps createSteps = new BusinessCreateSteps();
        
        private string SutName { get; set; }
        private string SutNameNew { get; set; }

        public Guid SutKey { get; private set; }
        public Business Sut { get; private set; }
        public IList<Business> Suts { get; private set; } = new List<Business>();
        public IList<Business> RecycleBin { get; private set; } = new List<Business>();

        public BusinessUpdateSteps()
        {
            _config = new AppConfigurationFactory().Create();
            _connectionString = _config[AppConfigurationKeys.SqlConnectionKey];
            _dbContext = new DbContextFactory(_connectionString).Create();
        }

        [TestMethod]
        public async Task Business_Update_Green()
        {
            await GivenAnExistingBusinessHasBeenQueried();
            GivenABusinessWasFoundInPersistence();
            WhenTheBusinessNameChanges();
            await WhenBusinessIsUpdatedViaEntityFramework();
            await ThenTheExistingBusinessCanBeQueriedByKey();
            ThenTheBusinessNameMatchesTheNewName();
        }

        public async Task GivenAnExistingBusinessHasBeenQueried()
        {
            createSteps.GivenANewBusinessHasBeenCreated();
            await createSteps.WhenBusinessIsInsertedViaEntityFramework();
            Sut = await _dbContext.Business.FirstAsync();
            SutKey = Sut.BusinessKey;
        }

        public void GivenABusinessWasFoundInPersistence()
        {
            Assert.IsTrue(Sut.BusinessKey != Guid.Empty);
        }

        public void WhenTheBusinessNameChanges()
        {
            SutKey = Sut.BusinessKey;
            SutName = Sut.BusinessName;
            SutNameNew = $"Business name as of {DateTime.UtcNow.ToShortTimeString()}";
            Sut.BusinessName = SutNameNew;
        }

        public async Task WhenBusinessIsUpdatedViaEntityFramework()
        {
            _dbContext.Entry(Sut).State = EntityState.Modified;
            var result = await _dbContext.SaveChangesAsync();
            Assert.IsTrue(result > 0);
        }

        public async Task ThenTheExistingBusinessCanBeQueriedByKey()
        {
            Sut = await _dbContext.Business.FirstOrDefaultAsync(x => x.BusinessKey == SutKey);
        }

        public void ThenTheBusinessNameMatchesTheNewName()
        {
            Assert.IsTrue(SutNameNew == Sut.BusinessName);
            Assert.IsFalse(SutNameNew == SutName);
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
