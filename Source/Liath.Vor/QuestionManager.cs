using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liath.Vor.Contracts.BusinessLogic;
using Liath.Vor.Contracts.DataAccess;
using Liath.Vor.Models;

namespace Liath.Vor
{
  public class QuestionManager : IQuestionManager
  {
    private readonly ISecurityManager _securityManager;
    private readonly ITimeManager _timeManager;
    private readonly IQuestionDataAccess _questionDataAccess;

    public QuestionManager(ISecurityManager securityManager, ITimeManager timeManager, IQuestionDataAccess questionDataAccess)
    {
      if (securityManager == null) throw new ArgumentNullException(nameof(securityManager));
      if (timeManager == null) throw new ArgumentNullException(nameof(timeManager));
      if (questionDataAccess == null) throw new ArgumentNullException(nameof(questionDataAccess));

      _securityManager = securityManager;
      _timeManager = timeManager;
      _questionDataAccess = questionDataAccess;
    }

    public Exam GetExam(int quizID)
    {
      var quiz = _questionDataAccess.GetQuiz(quizID, false, false);
      if (quiz == null) return null;

      var user = _securityManager.GetOrCreateUserAccount();
      var now = _timeManager.GetNow();
      return _questionDataAccess.GetOrCreateExam(user, quiz, now);
    }

    public Question RecordAnswer(int examId, int questionId, bool isForwards, IEnumerable<int> options)
    {
      var exam = _questionDataAccess.GetExam(examId, false);

      throw new NotImplementedException();
    }
  }
}
