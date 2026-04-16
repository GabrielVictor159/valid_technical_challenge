using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infraestructure.Data.Mappings;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("ord_int_id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.NumberOrder)
            .HasColumnName("ord_str_number_order")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CreatedDate)
            .HasColumnName("ord_date_created_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.Status)
            .HasColumnName("ord_int_status")
            .IsRequired();

        builder.Property(x => x.TotalPrice)
            .HasColumnName("ord_dec_total_price")
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasIndex(x => x.NumberOrder)
            .IsUnique();

        builder.HasMany(x => x.Operations)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}