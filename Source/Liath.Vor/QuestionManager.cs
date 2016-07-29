using System;
using System.Collections.Generic;
using System.Data;
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
      
      var answer = _questionDataAccess.GetOrCreateAnswer(exam, thisQuestion);
      _questionDataAccess.ClearExistingOptionsFromAnswer(answer);
      if (options != null)
      {
        foreach (var option in options)
        {
          _questionDataAccess.SaveOptionForAnswer(answer, option);
        }
      }

      var nextQuestion = GetNextQuestion(exam, thisQuestion, isForwards);
      if (nextQuestion != null && nextQuestion.QuestionID.HasValue)
      {
        exam.CurrentQuestionID = nextQuestion.QuestionID.Value;
        _questionDataAccess.UpdateExam(exam);
      }

      return nextQuestion;
    }

    public ExamResults SubmitExam(int examId)
    {
      var currentUser = _securityManager.GetOrCreateUserAccount();
      var exam = _questionDataAccess.GetExam(examId, true);
      if (currentUser.UserAccountID != exam.UserAccount.UserAccountID)
      {
        throw new DataException("The user is not authorised to view this exam");
      }           

      var results = new ExamResults
      {
        ExamID = examId,
        QuestionResults = exam.Quiz.Questions.Select(q => this.MarkQuestion(q, exam.Answers)),
        NumberOfQuestions = exam.Quiz.Questions.Count()
      };
      results.NumberOfCorrectAnswers = results.QuestionResults.Count(q => q.WasCorrect);

      exam.EndDate = _timeManager.GetNow();
      exam.Score = results.NumberOfCorrectAnswers;
      _questionDataAccess.UpdateExam(exam);

      return results;
    }

    /// <summary>
    /// Creates a QuestionResult indicating whether the exam question was answered correctly
    /// </summary>
    private QuestionResult MarkQuestion(Question question, IEnumerable<Answer> answers)
    {
      if (question == null) throw new ArgumentNullException(nameof(question));
      if (!question.QuestionID.HasValue) throw new ArgumentException("The question cannot be marked until it has been saved");
      if (answers == null) throw new ArgumentNullException(nameof(answers));

      bool isCorrect = false;
      var correctOptions = question.Options.Where(o => o.IsCorrect == true).OrderBy(o => o.OptionID);
      IEnumerable<Option> givenOptions = new Option[] {};

      var matchingAnswer = answers.SingleOrDefault(a => a.QuestionID == question.QuestionID.Value);
      if (matchingAnswer != null)
      {
        var givenOptionIDs = matchingAnswer.AnswerOptions.OrderBy(o => o.OptionID).Select(ao => ao.OptionID);
        givenOptions = question.Options.Where(o => o.OptionID.HasValue && givenOptionIDs.Contains(o.OptionID.Value));
        if (givenOptions.Count() == correctOptions.Count())
        {
          var allAnswersCorrectSoFar = true;
          for (int i = 0; i < givenOptions.Count(); i++)
          {
            if (givenOptionIDs.ElementAt(i) != correctOptions.ElementAt(i).OptionID)
            {
              allAnswersCorrectSoFar = false;
              break;
            }
          }

          isCorrect = allAnswersCorrectSoFar;
        }
      }

      
      return new QuestionResult
      {
        Question = question,
        WasCorrect = isCorrect,
        GivenOptions = givenOptions,
        CorrectOptions = correctOptions
      };
    }

    private Question GetNextQuestion(Exam exam, Question thisQuestion, bool isForwards)
    {
      var currentIndex = exam.Quiz.Questions.ToList().FindIndex(q => q.Equals(thisQuestion));
      if (isForwards)
      {
        if (currentIndex + 1 < exam.Quiz.Questions.Count())
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
        else
        {
          return exam.Quiz.Questions.First();
        }
      }

      return null;
    }
  }
}
