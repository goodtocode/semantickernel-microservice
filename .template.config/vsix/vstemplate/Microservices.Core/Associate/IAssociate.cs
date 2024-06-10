using GoodToCode.Shared.Domain;
using System;
using System.Collections.Generic;

namespace Microservice.Domain
{
    public interface IAssociate : IDomainEntity<IAssociate>
    {
        Guid AssociateKey { get; set; }
    }
}