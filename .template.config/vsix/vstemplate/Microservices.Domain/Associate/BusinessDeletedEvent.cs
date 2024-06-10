
using GoodToCode.Shared.Domain;
using $safeprojectname$;

namespace $safeprojectname$
{
    public sealed class BusinessDeletedEvent : IDomainEvent<IBusiness>
    {
        public IBusiness Item { get; }

        public BusinessDeletedEvent(IBusiness item)
        {
            Item = item;
        }
    }
}
