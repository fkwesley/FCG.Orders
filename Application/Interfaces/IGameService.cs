using Application.DTO.Game;

namespace Application.Interfaces
{
    public interface IGameService
    {
        GameResponse GetGameById(int id);
    }
}
