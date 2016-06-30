using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liath.Vor.Contracts.DataAccess;
using Liath.Vor.Models;

namespace Liath.Vor.DataAccess
{
  public class SecurityDataAccess : ISecurityDataAccess
  {
    public UserAccount GetOrCreateUserAccount(string domainName)
    {
      throw new NotImplementedException();
    }
  }
}
