using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(o => o.OrderId);
            builder.Property(o => o.OrderId).ValueGeneratedOnAdd().HasColumnType("INT");

            builder.Property(o => o.UserId)
                   .HasMaxLength(20)
                   .IsRequired(true);

            builder.Property(o => o.Status).IsRequired().HasColumnType("INT");
            builder.Property(o => o.PaymentMethod).IsRequired().HasColumnType("INT");

            builder.Property(o => o.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasConversion(
                    v => v, // Grava no banco normalmente
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Força Kind como UTC ao ler
                );
            builder.Property(o => o.UpdatedAt)
                .IsRequired(false)
                .HasConversion(
                    v => v, // Grava no banco normalmente  
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null // Força Kind como UTC ao ler  
                );

            builder.Ignore(o => o.PaymentMethodDetails);
            builder.Ignore(o => o.TotalPrice);
        }
    }
}
