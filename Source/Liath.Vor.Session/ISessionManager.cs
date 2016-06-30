namespace SDA.DevPortal.Session
{
    public interface ISessionManager
    {
        IUnitOfWork CreateUnitOfWork();
        IUnitOfWork GetCurrentUnitOfWork();
    }
}
