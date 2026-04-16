
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infraestructure.Data.Mappings;

public class OperationConfiguration : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.ToTable("Operations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("ope_int_id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.OrderId)
            .HasColumnName("ord_int_id")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("ope_int_status")
            .IsRequired();

        builder.Property(x => x.CreatedDate)
            .HasColumnName("ope_date_created_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.Message)
            .HasColumnName("ope_str_message")
            .IsRequired()
            .HasMaxLength(500);
    }
}