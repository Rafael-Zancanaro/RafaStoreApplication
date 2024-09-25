namespace RafaStore.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}