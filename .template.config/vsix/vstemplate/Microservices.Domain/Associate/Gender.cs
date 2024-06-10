using GoodToCode.Shared.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace $safeprojectname$
{
    public class Gender : DomainEntity<IGender>, IGender
    {
        public override string PartitionKey { get; set; } = "Default";
        public override Guid RowKey { get { return GenderKey; } set { GenderKey = value; } }
        public Guid GenderKey { get; set; }
        public string GenderName { get; set; }
        public string GenderCode { get; set; }
        
        
    }
}
