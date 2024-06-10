using GoodToCode.Shared.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace $safeprojectname$
{
    public class Government : DomainEntity<IGovernment>, IGovernment
    {
        public override string PartitionKey { get; set; } = "Default";
        public override Guid RowKey { get { return GovernmentKey; } set { GovernmentKey = value; } }
        public Guid GovernmentKey { get; set; }
        public string GovernmentName { get; set; }
        
        
    }
}
