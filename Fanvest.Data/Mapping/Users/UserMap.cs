using Fanvest.Core.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Fanvest.Data.Mapping.Users
{
    public partial class UserMap
    {
        public UserMap(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email).HasMaxLength(200);
            builder.Property(u => u.Username).HasMaxLength(200);
            builder.Property(u => u.FirstName).HasMaxLength(200);
            builder.Property(u => u.LastName).HasMaxLength(200);
            builder.Property(u => u.SystemName).HasMaxLength(200);
            builder.Property(u => u.Balance).HasColumnType("decimal(18,4)");
        }
    }
}