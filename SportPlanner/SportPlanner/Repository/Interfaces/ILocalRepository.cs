namespace SportPlanner.Repository.Interfaces
{
    public interface ILocalRepository<T>
    {
        bool DeleteEntity(string fileName);
        T GetEntity(string fileName);

        bool UpsertEntity(string fileName, T entity);
    }
}