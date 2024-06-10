using GoodToCode.Shared.dotNet.EntityFramework;
using Microservice.Domain;
using Microsoft.EntityFrameworkCore;

namespace $safeprojectname$
{
    public interface IAssociateDbContext : IDbContext
    {
        DbSet<Business> Business { get; set; }
        DbSet<Associate> Associate { get; set; }
        DbSet<Gender> Gender { get; set; }
        DbSet<Government> Government { get; set; }
        DbSet<Person> Person { get; set; }
    }
}