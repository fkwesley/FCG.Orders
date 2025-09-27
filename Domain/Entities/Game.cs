
using System.Diagnostics;

namespace Domain.Entities
{
    public class Game
    {
        [DebuggerDisplay("GameId: {GameId}, Name: {Name}, Price: {Price}")]
        public int GameId { get; set; }
        public string Name { get; set; }
        public int? OrderId { get; set; } // Chave estrangeira para Order
        public double Price { get; set; }
        public Order? Order { get; set; } = null; // Propriedade de navegação
    }
}
