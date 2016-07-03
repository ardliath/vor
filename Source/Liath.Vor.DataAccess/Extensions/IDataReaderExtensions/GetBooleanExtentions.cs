using System.Data;

namespace Liath.Vor.DataAccess.Extensions.IDataReaderExtensions
{
  public static class GetBooleanExtentions
  {
    public static bool GetBoolean(this IDataReader dr, string columnName)
    {
      var ordinal = dr.GetOrdinal(columnName);
      return dr.GetBoolean(ordinal);
    }
  }
}
