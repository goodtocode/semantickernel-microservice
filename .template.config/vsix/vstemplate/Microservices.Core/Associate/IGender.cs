using GoodToCode.Shared.Domain;
using System;

namespace Microservice.Domain
{
    public interface IGender : IDomainEntity<IGender>
    {
        string GenderCode { get; set; }
        Guid GenderKey { get; set; }
        string GenderName { get; set; }
    }
}