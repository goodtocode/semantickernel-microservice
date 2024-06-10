using GoodToCode.Shared.Domain;
using System;

namespace Microservice.Domain
{
    public interface IPerson : IDomainEntity<IPerson>
    {
        DateTime BirthDate { get; set; }
        string FirstName { get; set; }
        string GenderCode { get; set; }
        string LastName { get; set; }
        string MiddleName { get; set; }
        Guid PersonKey { get; set; }
    }
}