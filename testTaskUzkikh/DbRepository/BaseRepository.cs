using testTaskUzkikh.DbRepository.Interfaces;

namespace testTaskUzkikh.DbRepository
{
    public abstract class BaseRepository
    {
        public string ConnectionString { get; }
        public IRepositoryContextFactory RepositoryContextFactory { get; }

        public BaseRepository(string connectionString, IRepositoryContextFactory repositoryContextFactory)
        {
            ConnectionString = connectionString;
            RepositoryContextFactory = repositoryContextFactory;
        }
    }
}
