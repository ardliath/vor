using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Liath.Vor.UI.Web.Models.Question
{
  public class RecordAnswer
  {
    public int ExamID { get; set; }
    public int QuestionID { get; set; }
    public int[] Answers { get; set; }
  }
}