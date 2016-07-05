using System;
using System.Data;

namespace Liath.Vor.DataAccess.Extensions.IDataReaderExtensions
{
  public static class GetDateTimeExtensions
  {
    public static DateTime GetDateTime(this IDataReader dr, string columnName)
    {
      var ordinal = dr.GetOrdinal(columnName);
      return dr.GetDateTime(ordinal);
    }

    public static DateTime? GetNullableDateTime(this IDataReader dr, string columnName)
    {
      var ordinal = dr.GetOrdinal(columnName);
      return dr.IsDBNull(ordinal)
        ? null
        : (DateTime?)dr.GetDateTime(ordinal);
    }
  }
}