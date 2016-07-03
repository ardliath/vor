using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liath.Vor.Session;

namespace Liath.Vor.DataAccess.Extensions
{
  public static class IUnitOfWorkExtensions
  {
    public static IDbCommand CreateSPCommand(this IUnitOfWork unitOfWork, string spName)
    {
      var cmd = unitOfWork.CreateCommand(spName);
      cmd.CommandType = CommandType.StoredProcedure;
      return cmd;
    }
  }
}
