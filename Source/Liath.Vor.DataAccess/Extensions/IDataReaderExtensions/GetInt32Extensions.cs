using System.Data;

namespace Liath.Vor.DataAccess.Extensions.IDataReaderExtensions
{
  public static class GetInt32Extensions
  {
    public static int GetInt32(this IDataReader dr, string columnName)
    {
      var ordinal = dr.GetOrdinal(columnName);
      return dr.GetInt32(ordinal);
    }

    public static int? GetNullableInt32(this IDataReader dr, string columnName)
    {
      var ordinal = dr.GetOrdinal(columnName);
      return dr.IsDBNull(ordinal)
        ? null
        : (int?)dr.GetInt32(ordinal);
    }
  }
}
