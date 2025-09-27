
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IGameRepository
    {
        Game GetGameById(int id);
    }
}
