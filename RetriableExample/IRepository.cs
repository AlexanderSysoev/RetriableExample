using System.Threading.Tasks;

namespace RetriableExample
{
    public interface IRepository
    {
        Entity GetById(int id);

        Task<Entity> GetByIdAsync(int id);
    }
}
