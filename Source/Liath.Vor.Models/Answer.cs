using System.Collections.Generic;

namespace Liath.Vor.Models
{
  public class Answer
  {
    public int? AnswerID { get; set; }
    public int ExamID { get; set; }
    public int QuestionID { get; set; }

    public IEnumerable<AnswerOption> AnswerOptions { get; set; }
  }
}