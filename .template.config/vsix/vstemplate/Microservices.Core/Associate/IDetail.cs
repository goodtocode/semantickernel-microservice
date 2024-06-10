using GoodToCode.Shared.Domain;
using System;

namespace Microservice.Domain
{
    public interface IDetail : IDomainEntity<IDetail>
    {
        string DetailData { get; set; }
        Guid DetailKey { get; set; }
        Guid DetailTypeKey { get; set; }
        
    }
}