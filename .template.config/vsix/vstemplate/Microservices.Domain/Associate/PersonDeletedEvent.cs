
using GoodToCode.Shared.Domain;
using $safeprojectname$;

namespace $safeprojectname$
{
    public sealed class PersonDeletedEvent : IDomainEvent<IPerson>
    {
        public IPerson Item { get; }

        public PersonDeletedEvent(IPerson item)
        {
            Item = item;
        }
    }
}
