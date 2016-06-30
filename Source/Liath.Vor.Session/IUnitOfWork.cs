using System;
using System.Data;

namespace Liath.Vor.Session
{
    public interface IUnitOfWork : IDisposable
    {
        IDbCommand CreateCommand(string query);
    }
}
