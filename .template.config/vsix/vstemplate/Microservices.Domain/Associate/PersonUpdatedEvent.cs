using GoodToCode.Shared.Domain;

namespace $safeprojectname$
{
    public sealed class PersonUpdatedEvent : IDomainEvent<IPerson>
    {
        public IPerson Item { get; }

        public PersonUpdatedEvent(IPerson item)
        {
            Item = item;
        }
    }
}
