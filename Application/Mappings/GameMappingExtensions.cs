using Application.DTO.Game;
using Domain.Entities;

namespace FCG.Application.Mappings
{
    public static class GameMappingExtensions
    {
        /// <summary>
        /// maps a Game entity to a GameResponse.
        public static GameResponse ToResponse(this Game entity)
        {
            if (entity == null) 
                return null;

            return new GameResponse
            {
                GameId = entity.GameId,
                Name = entity.Name,
                Price = entity.Price
            };
        }
    }
}
