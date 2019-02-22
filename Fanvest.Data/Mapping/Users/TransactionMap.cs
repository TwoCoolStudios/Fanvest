using Fanvest.Core.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Fanvest.Data.Mapping.Users
{
    public partial class TransactionMap
    {
        public TransactionMap(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable(nameof(Transaction));
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Description).IsRequired();
            builder.Property(t => t.Credit).HasColumnType("decimal(18,4)");
            builder.Property(t => t.Debit).HasColumnType("decimal(18,4)");
            builder.Property(t => t.Balance).HasColumnType("decimal(18,4)");

            builder.HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .IsRequired();
        }
    }
}