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
      if (exam == null) return null;

      var thisQuestion = exam.Quiz.Questions.SingleOrDefault(q => q.QuestionID == questionId);
      if (thisQuestion == null) return null;

      // TODO: record the answer!
      throw new NotImplementedException();

      var nextQuestion = GetNextQuestion(exam, thisQuestion, isForwards);

      // TODO: save the next question position to the database
      return nextQuestion;
    }

    private Question GetNextQuestion(Exam exam, Question thisQuestion, bool isForwards)
    {
      var currentIndex = exam.Quiz.Questions.ToList().FindIndex(q => q.Equals(thisQuestion));
      if (isForwards)
      {
        if (currentIndex < exam.Quiz.Questions.Count())
        {
          return exam.Quiz.Questions.ElementAt(currentIndex + 1);
        }
      }
      else
      {
        if (currentIndex > 0)
        {
          return exam.Quiz.Questions.ElementAt(currentIndex - 1);
        }
      }

      return null;
    }
  }
}
