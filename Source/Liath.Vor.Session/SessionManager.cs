//using NLog;

//using NLog.Internal;

namespace Liath.Vor.Session
{
    public class SessionManager : ISessionManager
    {
        //private static ILogger logger = LogManager.GetCurrentClassLogger();
        private IUnitOfWork _unitOfWork;

        public IUnitOfWork GetCurrentUnitOfWork()
        {
            //logger.Trace("Getting UnitOfWork");
            if (_unitOfWork == null)
            {
                throw new NoUnitOfWorkException();                
            }
            return _unitOfWork;
        }

	    public IUnitOfWork CreateUnitOfWork()
	    {
		    //logger.Trace("Creating a new UnitOfWork");
		    var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SDA.DevPortal"];
		    _unitOfWork = new UnitOfWork(connectionString);
		    return _unitOfWork;
	    }
    }
}
