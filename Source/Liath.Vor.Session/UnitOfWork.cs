using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SDA.DevPortal.Session
{
    public class UnitOfWork : IUnitOfWork
    {        
        private IDbConnection _connection;
        private ConnectionStringSettings _connectionStringSettings;

        public UnitOfWork(ConnectionStringSettings connectionStringSettings)
        {
            if (connectionStringSettings == null) throw new ArgumentNullException(nameof(connectionStringSettings));
            _connectionStringSettings = connectionStringSettings;
        }

        public void Dispose()
        {
            if(_connection != null)
            {
                _connection.Dispose();
            }
        }

	    public IDbCommand CreateCommand(string query)
	    {
		    if (query == null) throw new ArgumentNullException(nameof(query));
		    var conn = GetConnection();
		    var cmd = conn.CreateCommand();
		    cmd.CommandText = query;
		    return cmd;
	    }

	    private IDbConnection GetConnection()
        {            
            if(_connection == null)
            {                
                _connection = this.CreateConnection();
            }
            return _connection;
        }

	    private IDbConnection CreateConnection()
	    {		    
		    var conn = new SqlConnection(_connectionStringSettings.ConnectionString);
		    conn.Open();
		    return conn;
	    }
    }
}
