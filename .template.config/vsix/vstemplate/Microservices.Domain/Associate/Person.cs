using GoodToCode.Shared.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace $safeprojectname$
{
    public class Person : DomainEntity<IPerson>, IPerson
    {
        public override string PartitionKey { get; set; } = "Default";
        public override Guid RowKey { get { return PersonKey; } set { PersonKey = value; } }
        public Guid PersonKey { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string GenderCode { get; set; }
        
        
    }
}
