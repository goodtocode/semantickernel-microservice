using GoodToCode.Shared.Domain;
using Microservice.Domain;
using Microservice.Persistence;
using Microsoft.EntityFrameworkCore;

namespace $safeprojectname$
{
    public class AssociateAggregate : DomainAggregate<AssociateAggregate>
    {
        private readonly IAssociateDbContext _dbContext;
        private int _recordsAffected;

        public AssociateAggregate(int recordsAffected)
        {
            _recordsAffected = recordsAffected;
        }

        public AssociateAggregate(IAssociateDbContext context)
        {
            _dbContext = context;
        }

        // Business
        public async Task<int> BusinessUpdateAsync(IBusiness business)
        {
            IDomainEvent<IBusiness> eventRaise;
            _dbContext.Entry((Business)business).State = EntityState.Modified;
            eventRaise = new BusinessUpdatedEvent(business);
            _recordsAffected = await _dbContext.SaveChangesAsync();            
            business.RaiseDomainEvent(eventRaise);
            return _recordsAffected;
        }
        public async Task<int> BusinessCreateAsync(IBusiness business)
        {
            IDomainEvent<IBusiness> eventRaise;
            _dbContext.Business.Add((Business)business);
            eventRaise = new BusinessCreatedEvent(business);
            _recordsAffected = await _dbContext.SaveChangesAsync();

            business.RaiseDomainEvent(eventRaise);
            return _recordsAffected;
        }
        public async Task<int> BusinessSaveAsync(IBusiness business)
        {
            // Record in local storage

            IDomainEvent<IBusiness> eventRaise;

            if (business.BusinessKey != Guid.Empty)
            {
                _dbContext.Entry((Business)business).State = EntityState.Modified;
                eventRaise = new BusinessUpdatedEvent(business);
            }
            else
            {
                _dbContext.Business.Add((Business)business);
                eventRaise = new BusinessCreatedEvent(business);
            }
            _recordsAffected = await _dbContext.SaveChangesAsync();
            business.RaiseDomainEvent(eventRaise);

            return _recordsAffected;
        }
        public async Task<int> BusinessDeleteAsync(Guid key)
        {
            // Record in local storage
            IDomainEvent<IBusiness> eventRaise;
            var business = await _dbContext.Business.FindAsync(key);


            if (business.BusinessKey != Guid.Empty)
            {
                _dbContext.Entry((Business)business).State = EntityState.Deleted;
                eventRaise = new BusinessUpdatedEvent(business);
                _recordsAffected = await _dbContext.SaveChangesAsync();
                business.RaiseDomainEvent(eventRaise);
            }

            return _recordsAffected;
        }

        // Person
        public async Task<int> PersonUpdateAsync(IPerson Person)
        {
            IDomainEvent<IPerson> eventRaise;
            _dbContext.Entry((Person)Person).State = EntityState.Modified;
            eventRaise = new PersonUpdatedEvent(Person);
            _recordsAffected = await _dbContext.SaveChangesAsync();
            Person.RaiseDomainEvent(eventRaise);
            return _recordsAffected;
        }
        public async Task<int> PersonCreateAsync(IPerson Person)
        {
            IDomainEvent<IPerson> eventRaise;
            _dbContext.Person.Add((Person)Person);
            eventRaise = new PersonCreatedEvent(Person);
            _recordsAffected = await _dbContext.SaveChangesAsync();

            Person.RaiseDomainEvent(eventRaise);
            return _recordsAffected;
        }
        public async Task<int> PersonSaveAsync(IPerson Person)
        {
            // Record in local storage

            IDomainEvent<IPerson> eventRaise;

            if (Person.PersonKey != Guid.Empty)
            {
                _dbContext.Entry((Person)Person).State = EntityState.Modified;
                eventRaise = new PersonUpdatedEvent(Person);
            }
            else
            {
                _dbContext.Person.Add((Person)Person);
                eventRaise = new PersonCreatedEvent(Person);
            }
            _recordsAffected = await _dbContext.SaveChangesAsync();
            Person.RaiseDomainEvent(eventRaise);

            return _recordsAffected;
        }
        public async Task<int> PersonDeleteAsync(Guid key)
        {
            // Record in local storage
            IDomainEvent<IPerson> eventRaise;
            var Person = await _dbContext.Person.FindAsync(key);


            if (Person.PersonKey != Guid.Empty)
            {
                _dbContext.Entry((Person)Person).State = EntityState.Deleted;
                eventRaise = new PersonUpdatedEvent(Person);
                _recordsAffected = await _dbContext.SaveChangesAsync();
                Person.RaiseDomainEvent(eventRaise);
            }

            return _recordsAffected;
        }

        // Government
        public async Task<int> GovernmentSaveAsync(IGovernment government)
        {
            // Record locally
            // raise event with data to persistence

            if (government.GovernmentKey != Guid.Empty)
                _dbContext.Entry(government).State = EntityState.Modified;
            else
                _dbContext.Government.Add((Government)government);
            _recordsAffected = await _dbContext.SaveChangesAsync();

            return _recordsAffected;
        }
    }
}
