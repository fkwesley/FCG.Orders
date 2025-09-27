
namespace Domain.Entities
{
    public class Game
    {
        public int GameId { get; set; }
        public int? OrderId { get; set; } // Chave estrangeira para Order
        public double Price { get; set; }
        public Order? Order { get; set; } = null; // Propriedade de navegação

    }
}
