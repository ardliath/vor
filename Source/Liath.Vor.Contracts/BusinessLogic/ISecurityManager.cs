using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.Vor.Contracts.BusinessLogic
{
  public interface ISecurityManager
  {
    void EnsureUserAccountExists();
  }
}
