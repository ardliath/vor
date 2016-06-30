using System;

namespace Liath.Vor.Session
{
    public class NoUnitOfWorkException : Exception        
    {       
        public NoUnitOfWorkException()
            : base("There is no current UnitOfWork")
        {
        }
    }
}
