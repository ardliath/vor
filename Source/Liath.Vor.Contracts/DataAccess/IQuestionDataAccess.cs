﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liath.Vor.Models;

namespace Liath.Vor.Contracts.DataAccess
{
  public interface IQuestionDataAccess
  {
    Question GetQuestion(int id);
  }
}
