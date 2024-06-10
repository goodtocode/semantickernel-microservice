using GoodToCode.Shared.Domain;
using System;

namespace Microservice.Domain
{
    public interface IBusiness: IDomainEntity<IBusiness>
    {
        Guid BusinessKey { get; set; }
        string BusinessName { get; set; }
        string TaxNumber { get; set; }
    }
}