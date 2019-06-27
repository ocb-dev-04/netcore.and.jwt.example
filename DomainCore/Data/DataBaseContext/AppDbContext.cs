using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DomainCore.Data.Entities.App;
using DomainCore.Data.Entities.Identity;

namespace DomainCore.Data.DataBaseContext
{
    public class AppDbContext : IdentityDbContext<AppIdentityUser>
    {
        #region Construct

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        #endregion

        #region DbSet's

        //  all DbSet are application tables

        public DbSet<Example> Example { get; set; }

        #endregion
    }
}
