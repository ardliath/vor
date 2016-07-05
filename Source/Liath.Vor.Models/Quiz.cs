using System;
using System.Collections.Generic;

namespace Liath.Vor.Models
{
  public class Quiz
  {
    public int? QuizID { get; set; }
    public string Title { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public IEnumerable<Question> Questions { get; set; }
  }
}