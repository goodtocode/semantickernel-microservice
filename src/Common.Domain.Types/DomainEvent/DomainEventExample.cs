//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Goodtocode.Domain.Types.Domain
//{
//    public class OrderStartedDomainEvent : INotification
//    {
//        public string UserId { get; }
//        public int CardTypeId { get; }
//        public string CardNumber { get; }
//        public string CardSecurityNumber { get; }
//        public string CardHolderName { get; }
//        public DateTime CardExpiration { get; }

//        public OrderStartedDomainEvent(int cardTypeId, string cardNumber,
//                                       string cardSecurityNumber, string cardHolderName,
//                                       DateTime cardExpiration)
//        {
//            CardTypeId = cardTypeId;
//            CardNumber = cardNumber;
//            CardSecurityNumber = cardSecurityNumber;
//            CardHolderName = cardHolderName;
//            CardExpiration = cardExpiration;
//        }
//    }

//    public class domainobject
//    {
//        public void example()
//        {
//            var orderStartedDomainEvent = new OrderStartedDomainEvent(this, //Order object
//                                                          cardTypeId, cardNumber,
//                                                          cardSecurityNumber,
//                                                          cardHolderName,
//                                                          cardExpiration);
//            this.AddDomainEvent(orderStartedDomainEvent);
//        }
//    }

//    public class OrderingContext : DbContext, IUnitOfWork
//    {
//         ...
//        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
//        {
//             Dispatch Domain Events collection.
//             Choices:
//             A) Right BEFORE committing data (EF SaveChanges) into the DB. This makes
//             a single transaction including side effects from the domain event
//             handlers that are using the same DbContext with Scope lifetime
//             B) Right AFTER committing data (EF SaveChanges) into the DB. This makes
//             multiple transactions. You will need to handle eventual consistency and
//             compensatory actions in case of failures.
//            await _mediator.DispatchDomainEventsAsync(this);

//             After this line runs, all the changes (from the Command Handler and Domain
//             event handlers) performed through the DbContext will be committed
//            var result = await base.SaveChangesAsync();
//        }
//    }

//    public abstract class Entity
//    {
//        ...
//        private List<INotification> _domainEvents;
//        public List<INotification> DomainEvents => _domainEvents;

//        public void AddDomainEvent(INotification eventItem)
//        {
//            _domainEvents = _domainEvents ?? new List<INotification>();
//            _domainEvents.Add(eventItem);
//        }

//        public void RemoveDomainEvent(INotification eventItem)
//        {
//            _domainEvents?.Remove(eventItem);
//        }
//        ... Additional code
//    }


//     IoC Dispatcher Example

//    public class MediatorModule : Autofac.Module
//    {
//        protected override void Load(ContainerBuilder builder)
//        {
//            // Other registrations ...
//            // Register the DomainEventHandler classes (they implement IAsyncNotificationHandler<>)
//            // in assembly holding the Domain Events
//            builder.RegisterAssemblyTypes(typeof(ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler)
//                                           .GetTypeInfo().Assembly)
//                                             .AsClosedTypesOf(typeof(IAsyncNotificationHandler<>));
//            // Other registrations ...
//        }
//    }

//    public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler
//                   : INotificationHandler<OrderStartedDomainEvent>
//    {
//        private readonly ILoggerFactory _logger;
//        private readonly IBuyerRepository<Buyer> _buyerRepository;
//        private readonly IIdentityService _identityService;

//        public ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(
//            ILoggerFactory logger,
//            IBuyerRepository<Buyer> buyerRepository,
//            IIdentityService identityService)
//        {
//            // ...Parameter validations...
//        }

//        public async Task Handle(OrderStartedDomainEvent orderStartedEvent)
//        {
//            var cardTypeId = (orderStartedEvent.CardTypeId != 0) ? orderStartedEvent.CardTypeId : 1;
//            var userGuid = _identityService.GetUserIdentity();
//            var buyer = await _buyerRepository.FindAsync(userGuid);
//            bool buyerOriginallyExisted = (buyer == null) ? false : true;

//            if (!buyerOriginallyExisted)
//            {
//                buyer = new Buyer(userGuid);
//            }

//            buyer.VerifyOrAddPaymentMethod(cardTypeId,
//                                           $"Payment Method on {DateTime.UtcNow}",
//                                           orderStartedEvent.CardNumber,
//                                           orderStartedEvent.CardSecurityNumber,
//                                           orderStartedEvent.CardHolderName,
//                                           orderStartedEvent.CardExpiration,
//                                           orderStartedEvent.Order.Id);

//            var buyerUpdated = buyerOriginallyExisted ? _buyerRepository.Update(buyer)
//                                                                          : _buyerRepository.Add(buyer);

//            await _buyerRepository.UnitOfWork
//                    .SaveEntitiesAsync();

//            // Logging code using buyerUpdated info, etc.
//        }
//    }
//}
