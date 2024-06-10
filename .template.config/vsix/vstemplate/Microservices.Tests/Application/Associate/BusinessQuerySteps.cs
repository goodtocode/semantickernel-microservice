using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
    public class BusinessQuerySteps : IQuerySteps<Business>
    {        
        private readonly AssociateDbContext _dbContext;
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        private readonly BusinessSaveCommandSteps commandSteps = new BusinessSaveCommandSteps();

        public Guid SutKey { get; private set; }
        public IList<Business> Sut { get; private set; } = new List<Business>();
        public IList<Business> RecycleBin { get; private set; } = new List<Business>();

        public BusinessQuerySteps()
        {
            _config = new AppConfigurationFactory().Create();
            _connectionString = _config[AppConfigurationKeys.SqlConnectionKey];
            _dbContext = new DbContextFactory(_connectionString).Create();
        }

        [TestMethod]
        public async Task Business_Query_Green()
        {
            await GivenIHaveABusinessKeyThatCanBeQueried();
            await WhenBusinessIsReadByKeyViaQueryAsync();
            WhenTheBusinessExistsInQuery();
            ThenTheMatchingBusinessIsReturnedFromTheQuery();
        }

        public async Task GivenIHaveABusinessKeyThatCanBeQueried()
        {
            commandSteps.GivenANewBusinessSaveCommandHasBeenCreated();
            await commandSteps.WhenTheBusinessIsInsertedViaCQRSCommand();
            Sut.Add(await _dbContext.Business.FirstOrDefaultAsync());
            SutKey = Sut.FirstOrDefault().BusinessKey;
            Assert.IsTrue(SutKey != Guid.Empty);
        }

        public async Task WhenBusinessIsReadByKeyViaQueryAsync()
        {
            var query = new BusinessGetQuery(SutKey);
            var handle = new BusinessGetHandler(_dbContext);
            var response = await handle.Handle(query, new System.Threading.CancellationToken());
            Sut.Add(response.Result);
        }

        public void WhenTheBusinessExistsInQuery()
        {
            Assert.IsFalse(Sut == null);
        }

        public void ThenTheMatchingBusinessIsReturnedFromTheQuery()
        {
            Assert.IsTrue(Sut.Any(x => x.BusinessKey == SutKey));
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
