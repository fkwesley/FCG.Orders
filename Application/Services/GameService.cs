using Application.DTO.Game;
using Application.Interfaces;
using Domain.Repositories;
using FCG.Application.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FCG.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILoggerService _loggerService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IServiceScopeFactory _scopeFactory;

        public GameService(
                IGameRepository gameRepository, 
                ILoggerService loggerService,
                IHttpContextAccessor httpContext,
                IServiceScopeFactory scopeFactory)
        {
            _gameRepository = gameRepository 
                ?? throw new ArgumentNullException(nameof(gameRepository));
            _loggerService = loggerService;
            _httpContext = httpContext;
            _scopeFactory = scopeFactory;
        }

        public GameResponse GetGameById(int id)
        {
            var gameFound = _gameRepository.GetGameById(id);

            return gameFound.ToResponse();
        }
    }
}
