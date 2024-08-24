using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWT_AuthAndRefrest.Models.Entities.Settings
{
    public class RefreshConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            //entity.HasKey(e => e.IdHistorialToken).HasName("PK__Historia__03DC48A5BDFD22AD");
            builder.ToTable("RefreshToken");

            builder.Property(e => e.EsActivo).HasComputedColumnSql("(case when [ExpirationDate]<getdate() then CONVERT([bit],(0)) else CONVERT([bit], (1)) end)", false);
            builder.Property(e => e.CreationDate).HasColumnType("datetime");
            builder.Property(e => e.ExpirationDate).HasColumnType("datetime");
            builder.Property(e => e.RefreshTokenS)
                .HasMaxLength(200)
                .IsUnicode(false);
            builder.Property(e => e.Token)
                .HasMaxLength (500)
                .IsUnicode (false);
        }
    }
}
