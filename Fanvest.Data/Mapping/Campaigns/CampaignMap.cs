using Fanvest.Core.Models.Campaigns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Fanvest.Data.Mapping.Campaigns
{
    public partial class CampaignMap
    {
        public CampaignMap(EntityTypeBuilder<Campaign> builder)
        {
            builder.ToTable(nameof(Campaign));
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Slug).HasMaxLength(200);
            builder.Property(c => c.ImagePath).HasMaxLength(int.MaxValue);
            builder.Property(c => c.Price).HasColumnType("decimal(18,4)");
        }
    }
}