using System.Collections.Generic;

namespace Liath.Vor.Models
{
  public class QuestionResult
  {
    public Question Question { get; set; }
    public bool WasCorrect { get; set; }
    public IEnumerable<Option> GivenOptions { get; set; }
    public IEnumerable<Option> CorrectOptions { get; set; }
  }
}