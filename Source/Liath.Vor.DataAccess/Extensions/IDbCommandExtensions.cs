using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.Vor.DataAccess.Extensions
{
  public static class IDbCommandExtensions
  {
    public static IDbCommand CreateAndAddParameter(this IDbCommand cmd, string name, DbType dbType, object value)
    {
      var param = cmd.CreateParameter();
      param.ParameterName = name;      
      param.Value = value ?? DBNull.Value;
      param.DbType = dbType;
      cmd.Parameters.Add(param);

      return cmd;
    }
  }
}
