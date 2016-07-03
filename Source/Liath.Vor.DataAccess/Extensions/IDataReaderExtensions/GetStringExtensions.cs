using System.Data;

namespace Liath.Vor.DataAccess.Extensions.IDataReaderExtensions
{
  public static class GetStringExtensions
  {
    public static string GetString(this IDataReader dr, string columnName, bool allowNull = false)
    {
      var ordinal = dr.GetOrdinal(columnName);
      return allowNull && dr.IsDBNull(ordinal) 
        ? null
        : dr.GetString(ordinal);
    }
  }
}
