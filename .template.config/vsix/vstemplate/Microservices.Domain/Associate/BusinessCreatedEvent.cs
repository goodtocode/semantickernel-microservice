using GoodToCode.Shared.Domain;
using $safeprojectname$;

namespace $safeprojectname$
{
    public sealed class BusinessCreatedEvent : IDomainEvent<IBusiness>
    {
        public IBusiness Item { get; }

        public BusinessCreatedEvent(IBusiness item)
        {
            Item = item;
        }
    }
}
