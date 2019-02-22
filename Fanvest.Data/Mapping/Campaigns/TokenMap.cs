using Fanvest.Core.Models.Campaigns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Fanvest.Data.Mapping.Campaigns
{
    public partial class TokenMap
    {
        public TokenMap(EntityTypeBuilder<Token> builder)
        {
            builder.ToTable(nameof(Token));
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Amount).HasColumnType("decimal(18,4)");

            builder.HasOne(t => t.Campaign)
                .WithMany(c => c.Tokens)
                .HasForeignKey(t => t.CampaignId)
                .IsRequired();
            builder.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .IsRequired();
        }
    }
}