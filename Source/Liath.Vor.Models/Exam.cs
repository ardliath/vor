using System;

namespace Liath.Vor.Models
{
  public class Exam
  {
    public int? ExamID { get; set; }
    public int CurrentQuestionID { get; set; }
    public Quiz Quiz { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Score { get; set; }
    public UserAccount UserAccount { get; set; }
  }
}