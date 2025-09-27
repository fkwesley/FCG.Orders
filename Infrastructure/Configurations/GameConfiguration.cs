using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("Game");
            builder.HasKey(g => g.GameId);
            builder.Property(g => g.GameId).ValueGeneratedOnAdd().HasColumnType("INT");

            builder.Property(g => g.Price).IsRequired().HasColumnType("DECIMAL(18,2)");

            builder.HasOne(g => g.Order)                   // Cada Game pertence a uma Order
                       .WithMany(o => o.ListOfGames)       // Uma Order pode ter vários Games
                       .HasForeignKey(g => g.OrderId)      // OrderId é a FK em Game
                       .OnDelete(DeleteBehavior.Restrict); // Evita delete em cascata
        }
    }
}
