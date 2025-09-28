using Application.DTO.Game;
using Application.Interfaces;
using Domain.Repositories;
using FCG.Application.Mappings;

namespace FCG.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository 
                ?? throw new ArgumentNullException(nameof(gameRepository));
        }

        public GameResponse GetGameById(int id)
        {
            var gameFound = _gameRepository.GetGameById(id);

            return gameFound.ToResponse();
        }
    }
}
