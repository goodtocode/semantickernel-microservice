using GoodToCode.Shared.Domain;
using System;

namespace Microservice.Domain
{
    public interface ISettingType : IDomainEntity<ISettingType>
    {
        Guid SettingTypeKey { get; set; }
        string SettingTypeName { get; set; }
    }
}