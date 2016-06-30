namespace Liath.Vor.Session
{
    public interface ISessionManager
    {
        IUnitOfWork CreateUnitOfWork();
        IUnitOfWork GetCurrentUnitOfWork();
    }
}
