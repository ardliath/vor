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
using Liath.Vor.DataAccess.Extensions.IDataReaderExtensions;

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
      using (var cmd = _sessionManager.CreateUnitOfWork().CreateSPCommand("USR_GetOrCreateUser"))
      {
        using (var dr = cmd.CreateAndAddParameter("DomainName", DbType.String, domainName).ExecuteReader())
        {
          if (dr.Read())
          {
            var user = new UserAccount();
            user.UserAccountID = dr.GetInt32("UserAccountID");
            user.DomainName = dr.GetString("DomainName");
            user.Firstname = dr.GetString("Firstname", true);
            user.Lastname = dr.GetString("Lastname", true);

            return user;
          }
        }
      }

      return null;
    }
  }
}
