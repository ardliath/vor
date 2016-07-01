using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Liath.Vor.Contracts.BusinessLogic;
using Liath.Vor.Contracts.DataAccess;

namespace Liath.Vor
{
  public class SecurityManager : ISecurityManager
  {
    private readonly ISecurityDataAccess _securityDataAccess;

    public SecurityManager(ISecurityDataAccess securityDataAccess)
    {
      if (securityDataAccess == null) throw new ArgumentNullException(nameof(securityDataAccess));
      _securityDataAccess = securityDataAccess;
    }

    public void EnsureUserAccountExists()
    {
      var domainName = Thread.CurrentPrincipal?.Identity?.IsAuthenticated ?? false
        ? Thread.CurrentPrincipal?.Identity?.Name
        : null;

      _securityDataAccess.GetOrCreateUserAccount(domainName);
    }
  }
}
