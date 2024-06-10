using GoodToCode.Shared.Domain;
using $safeprojectname$;

namespace $safeprojectname$
{
    public sealed class PersonCreatedEvent : IDomainEvent<IPerson>
    {
        public IPerson Item { get; }

        public PersonCreatedEvent(IPerson item)
        {
            Item = item;
        }
    }
}
