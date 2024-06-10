using GoodToCode.Shared.Domain;
using $safeprojectname$;

namespace $safeprojectname$
{
    public sealed class BusinessUpdatedEvent : IDomainEvent<IBusiness>
    {
        public IBusiness Item { get; }

        public BusinessUpdatedEvent(IBusiness item)
        {
            Item = item;
        }
    }
}
