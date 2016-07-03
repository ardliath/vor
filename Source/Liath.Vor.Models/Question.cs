using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.Vor.Models
{
  public class Question
  {
    public int? QuestionID { get; set; }
    public UserAccount CreatedBy { get; set; }
    public string Text { get; set; }
    public QuestionType Type { get; set; }
    public IEnumerable<Option>  Options { get; set; }
  }
}
