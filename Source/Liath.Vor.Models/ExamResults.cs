using System.Collections.Generic;

namespace Liath.Vor.Models
{
  public class ExamResults
  {
    public int ExamID { get; set; }
    public IEnumerable<QuestionResult> QuestionResults { get; set; }
    public int NumberOfQuestions { get; set; }
    public int NumberOfCorrectAnswers { get; set; }
  }
}