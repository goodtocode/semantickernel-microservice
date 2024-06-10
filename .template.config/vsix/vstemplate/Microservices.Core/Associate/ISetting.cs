using GoodToCode.Shared.Domain;
using System;

namespace Microservice.Domain
{
    public interface ISetting : IDomainEntity<ISetting>
    {
        Guid SettingKey { get; set; }
        string SettingName { get; set; }
        int SettingTypeKey { get; set; }
        string SettingValue { get; set; }
    }
}