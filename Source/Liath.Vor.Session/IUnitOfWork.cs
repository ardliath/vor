using System;
using System.Data;

namespace SDA.DevPortal.Session
{
    public interface IUnitOfWork : IDisposable
    {
        IDbCommand CreateCommand(string query);
    }
}
