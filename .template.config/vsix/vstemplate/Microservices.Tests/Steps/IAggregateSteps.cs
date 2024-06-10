using GoodToCode.Shared.Domain;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public interface IAggregateSteps<T> where T : IDomainAggregate<T>
    {
        T Aggregate { get; }
        Task Cleanup();
    }
}
