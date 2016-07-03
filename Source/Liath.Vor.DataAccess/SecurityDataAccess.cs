using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liath.Vor.Contracts.DataAccess;
using Liath.Vor.DataAccess.Extensions;
using Liath.Vor.Models;
using Liath.Vor.Session;

namespace Liath.Vor.DataAccess
{
  public class SecurityDataAccess : ISecurityDataAccess
  {
    private readonly ISessionManager _sessionManager;
    public SecurityDataAccess(ISessionManager sessionManager)
    {
      if (sessionManager == null) throw new ArgumentNullException(nameof(sessionManager));
      _sessionManager = sessionManager;
    }

    public UserAccount GetOrCreateUserAccount(string domainName)
    {
      using (var cmd = _sessionManager.CreateUnitOfWork().CreateCommand("USR_GetOrCreateUser"))
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CreateAndAddParameter("DomainName", DbType.String, domainName);          

        using (var dr = cmd.ExecuteReader())
        {
          if (dr.Read())
          {
            var user = new UserAccount();
            user.UserAccountID = dr.GetInt32(0);
            user.DomainName = dr.GetString(1);
            user.Firstname = dr.IsDBNull(2) ? null : dr.GetString(2);
            user.Lastname = dr.IsDBNull(3) ? null : dr.GetString(3);

            return user;
          }
        }
      }

      return null;
    }
  }
}
