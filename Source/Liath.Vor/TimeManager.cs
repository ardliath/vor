using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liath.Vor.Contracts.BusinessLogic;

namespace Liath.Vor
{
  public class TimeManager : ITimeManager
  {
    public DateTime GetNow()
    {
      return DateTime.UtcNow;
    }
  }
}
