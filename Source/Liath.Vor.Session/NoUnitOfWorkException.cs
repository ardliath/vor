using System;

namespace SDA.DevPortal.Session
{
    public class NoUnitOfWorkException : Exception        
    {       
        public NoUnitOfWorkException()
            : base("There is no current UnitOfWork")
        {
        }
    }
}
