using GoodToCode.Shared.Domain;

using System;
using System.ComponentModel.DataAnnotations;

namespace $safeprojectname$
{
    public class Associate : DomainEntity<IAssociate>, IAssociate
    {
        public override string PartitionKey { get; set; } = "Default";
        public override Guid RowKey { get { return AssociateKey; } set { AssociateKey = value; } }
        public Guid AssociateKey { get; set; }                
    }
}
