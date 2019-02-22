using Fanvest.Core;
using Fanvest.Core.Models.Campaigns;
using Fanvest.Core.Models.Users;
using Fanvest.Data.Mapping.Campaigns;
using Fanvest.Data.Mapping.Users;
using Microsoft.EntityFrameworkCore;
namespace Fanvest.Data
{
    public partial class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            new CampaignMap(builder.Entity<Campaign>());
            new TokenMap(builder.Entity<Token>());
            new RoleMap(builder.Entity<Role>());
            new TransactionMap(builder.Entity<Transaction>());
            new UserMap(builder.Entity<User>());
            new UserPasswordMap(builder.Entity<UserPassword>());
            new UserRoleMap(builder.Entity<UserRole>());
            base.OnModelCreating(builder);
        }

        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity
            : BaseEntity => base.Set<TEntity>();
    }
}