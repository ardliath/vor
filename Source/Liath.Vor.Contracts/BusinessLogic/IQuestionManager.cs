using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liath.Vor.Models;

namespace Liath.Vor.Contracts.BusinessLogic
{
  public interface IQuestionManager
  {
    Exam GetExam(int quizID);
    Question RecordAnswer(int examId, int questionId, bool isForwards, IEnumerable<int> options);
    ExamResults SubmitExam(int examId);
  }
}
