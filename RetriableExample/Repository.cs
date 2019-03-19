using System.Threading.Tasks;

namespace RetriableExample
{
    public class Repository : IRepository
    {
        private int _exceptionsCoutner;

        public Entity GetById(int id)
        {
            if (_exceptionsCoutner <= 2)
            {
                _exceptionsCoutner++;
                throw new DatabaseException();
            }

            //read from db
            return new Entity {Id = id};
        }

        public async Task<Entity> GetByIdAsync(int id)
        {
            if (_exceptionsCoutner <= 2)
            {
                _exceptionsCoutner++;
                throw new DatabaseException();
            }

            //read from db
            return await Task.FromResult(new Entity { Id = id });
        }
    }
}
