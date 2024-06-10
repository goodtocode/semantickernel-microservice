using GoodToCode.Shared.Domain;
using System;

namespace Microservice.Domain
{
    public interface IGovernment : IDomainEntity<IGovernment>
    {
        Guid GovernmentKey { get; set; }
        string GovernmentName { get; set; }
    }
}