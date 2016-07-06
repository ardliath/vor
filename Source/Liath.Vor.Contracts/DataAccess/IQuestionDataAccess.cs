using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liath.Vor.Models;

namespace Liath.Vor.Contracts.DataAccess
{
  public interface IQuestionDataAccess
  {
    Question GetQuestion(int id, bool loadIsCorrect);
    Quiz GetQuiz(int id, bool loadIsCorrect, bool includeExpiredQuizes);
    Exam GetOrCreateExam(UserAccount user, Quiz quiz, DateTime now);
    Exam GetExam(int examId, bool loadIsCorrect);
  }
}
