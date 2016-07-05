using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Liath.Vor.Contracts.BusinessLogic;
using Liath.Vor.Contracts.DataAccess;
using Liath.Vor.Models;

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
      var domainName = GetDomainName();
      _securityDataAccess.GetOrCreateUserAccount(domainName);
    }

    private string GetDomainName()
    {
      return Thread.CurrentPrincipal?.Identity?.IsAuthenticated ?? false
        ? Thread.CurrentPrincipal?.Identity?.Name
        : null;
    }

    public UserAccount GetOrCreateUserAccount()
    {
      var domainName = GetDomainName();
      return _securityDataAccess.GetOrCreateUserAccount(domainName);
    }
  }
}
