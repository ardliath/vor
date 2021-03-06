﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liath.Vor.Contracts.DataAccess;
using Liath.Vor.DataAccess.Extensions;
using Liath.Vor.Models;
using Liath.Vor.Session;
using Liath.Vor.DataAccess.Extensions.IDataReaderExtensions;

namespace Liath.Vor.DataAccess
{
  public class QuestionDataAccess : IQuestionDataAccess
  {
    private readonly ISessionManager _sessionManager;
    private readonly ISecurityDataAccess _securityDataAccess;

    public QuestionDataAccess(ISessionManager sessionManager, ISecurityDataAccess securityDataAccess)
    {
      if (sessionManager == null) throw new ArgumentNullException(nameof(sessionManager));
      if (securityDataAccess == null) throw new ArgumentNullException(nameof(securityDataAccess));

      _sessionManager = sessionManager;
      _securityDataAccess = securityDataAccess;
    }

    public Question GetQuestion(int id, bool loadIsCorrect)
    {
      Question question = null;
      using (var cmd = _sessionManager.GetCurrentUnitOfWork().CreateSPCommand("QA_GetQuestion"))
      {
        using (var dr = cmd.CreateAndAddParameter("QuestionID", DbType.Int32, id).ExecuteReader())
        {
          if (dr.Read())
          {
            question = new Question();
            question.QuestionID = dr.GetInt32("QuestionID");
            question.Text = dr.GetString("Text");
            var typeText = dr.GetString("Type");
            QuestionType qType;
            if (!Enum.TryParse<QuestionType>(typeText, out qType))
            {
              return null;
            }
            question.Type = qType;

            dr.NextResult();
            if (dr.Read())
            {
              question.CreatedBy = new UserAccount();
              question.CreatedBy.UserAccountID = dr.GetInt32("UserAccountID");
              question.CreatedBy.DomainName = dr.GetString("DomainName");
              question.CreatedBy.Firstname = dr.GetString("Firstname", true);
              question.CreatedBy.Lastname = dr.GetString("Lastname", true);
            }

            dr.NextResult();
            var options = new List<Option>();
            while (dr.Read())
            {              
              var option = new Option();
              option.OptionID = dr.GetInt32("OptionID");
              option.Text = dr.GetString("Text");
              if (loadIsCorrect)
              {
                option.IsCorrect = dr.GetBoolean("IsCorrect");
              }
              options.Add(option);
            }
            question.Options = options;
          }
        }
      }

      return question;
    }

    public Quiz GetQuiz(int id, bool loadIsCorrect, bool includeExpiredQuizes)
    {
      Quiz quiz = null;
      using (var cmd = _sessionManager.GetCurrentUnitOfWork().CreateSPCommand("QA_GetQuiz"))
      {
        using (var dr = cmd.CreateAndAddParameter("QuizID", DbType.Int32, id).ExecuteReader())
        {
          if (dr.Read())
          {
            quiz = new Quiz();
            quiz.QuizID = dr.GetInt32("QuizID");
            quiz.Title = dr.GetString("Title");
            quiz.StartDate = dr.GetNullableDateTime("StartDate");
            quiz.EndDate = dr.GetNullableDateTime("EndDate");

            if (!includeExpiredQuizes)
            {
              var now = DateTime.UtcNow;
              if (quiz.StartDate.HasValue && quiz.StartDate.Value > now)
              {
                return null;
              }

              if (quiz.EndDate.HasValue && quiz.EndDate.Value < now)
              {
                return null;
              }
            }
          }
          else
          {
            return null;
          }

          dr.NextResult();
          Question question = null;
          var questions = new List<Question>();
          var questionCreatedBy = new Dictionary<int, int>();
          while (dr.Read())
          {
            question = new Question();
            question.QuestionID = dr.GetInt32("QuestionID");
            question.Text = dr.GetString("Text");
            var typeText = dr.GetString("Type");
            QuestionType qType;
            if (!Enum.TryParse<QuestionType>(typeText, out qType))
            {
              continue;
            }
            question.Type = qType;
            questions.Add(question);
            questionCreatedBy.Add(question.QuestionID.Value, dr.GetInt32("CreatedBy"));
          }
          quiz.Questions = questions;

          dr.NextResult();
          UserAccount user = null;
          var users = new List<UserAccount>();
          while (dr.Read())
          {
            user = new UserAccount();
            user.UserAccountID = dr.GetInt32("UserAccountID");
            user.DomainName = dr.GetString("DomainName");
            user.Firstname = dr.GetString("Firstname", true);
            user.Lastname = dr.GetString("Lastname", true);

            users.Add(user);
          }

          dr.NextResult();
          var options = new Dictionary<int, List<Option>>();
          while (dr.Read())
          {
            var questionID = dr.GetInt32("QuestionID");
            if (!options.ContainsKey(questionID))
            {
              options[questionID] = new List<Option>();
            }

            var option = new Option();
            option.OptionID = dr.GetInt32("OptionID");
            option.Text = dr.GetString("Text");
            if (loadIsCorrect)
            {
              option.IsCorrect = dr.GetBoolean("IsCorrect");
            }
            options[questionID].Add(option);
          }

          foreach (var thisQuestion in quiz.Questions)
          {
            thisQuestion.Options = options.ContainsKey(thisQuestion.QuestionID.Value)
              ? options[thisQuestion.QuestionID.Value]
              : new List<Option>();

            if (questionCreatedBy.ContainsKey(thisQuestion.QuestionID.Value))
            {
              var userID = questionCreatedBy[thisQuestion.QuestionID.Value];
              thisQuestion.CreatedBy = users.SingleOrDefault(u => u.UserAccountID == userID);
            }
          }
        }        
      }

      return quiz;
    }

    public Exam GetOrCreateExam(UserAccount user, Quiz quiz, DateTime now)
    {
      if (user == null) throw new ArgumentNullException(nameof(user));
      if (quiz == null) throw new ArgumentNullException(nameof(quiz));

      if(!quiz.QuizID.HasValue) throw new ArgumentNullException("The quiz must be saved before it can be used as an exam");
      if (!user.UserAccountID.HasValue) throw new ArgumentNullException("The user must be saved before they can take an exam");

      using (var cmd = _sessionManager.GetCurrentUnitOfWork().CreateSPCommand("QA_GetOrCreateExam"))
      {
        var firstQuestion = quiz.Questions.FirstOrDefault();
        if (firstQuestion == null)
        {
          throw new InvalidDataException("The quiz does not have any questions");
        }
        if (!firstQuestion.QuestionID.HasValue)
        {
          throw new InvalidDataException("The question has not been saved");
        }

        using (var dr = cmd.CreateAndAddParameter("QuizID", DbType.Int32, quiz.QuizID.Value)
          .CreateAndAddParameter("UserAccountID", DbType.Int32, user.UserAccountID)
          .CreateAndAddParameter("Now", DbType.DateTime, now)
          .CreateAndAddParameter("FirstQuestion", DbType.Int32, firstQuestion.QuestionID.Value)
          .ExecuteReader())
        {

          var exam = new Exam();
          if (dr.Read())
          {
            exam.ExamID = dr.GetInt32("ExamID");
            exam.CurrentQuestionID = dr.GetInt32("QuestionID");
            exam.StartDate = dr.GetDateTime("StartDate");
            exam.EndDate = dr.GetNullableDateTime("EndDate");
            exam.Score = dr.GetNullableInt32("Score");
            exam.Quiz = quiz;
            exam.UserAccount = user;
          }
          else
          {
            return null;
          }


          dr.NextResult();
          this.ReadAnswers(exam, dr);

          return exam;
          
        }
      }        
    }

    private void ReadAnswers(Exam exam, IDataReader dr)
    {      
      var qa = new Dictionary<int, List<AnswerOption>>();
      var questions = new List<int>();
      while (dr.Read())
      {
        var questionID = dr.GetInt32("QuestionID");
        var answerID = dr.GetInt32("AnswerID");
        var optionID = dr.GetInt32("OptionID");

        if (!qa.ContainsKey(questionID))
        {
          qa.Add(questionID, new List<AnswerOption>());
          questions.Add(questionID);
        }
        qa[questionID].Add(new AnswerOption
        {
          QuestionID = questionID,
          AnswerID = answerID,
          OptionID = optionID
        });
      }

      var answers = new List<Answer>();
      foreach (var question in questions)
      {
        answers.Add(new Answer
        {
          QuestionID = question,
          ExamID = exam.ExamID.Value,
          AnswerID = qa[question].First().AnswerID, // there's only one answer per question per exam so we can take the first
          AnswerOptions = qa[question].Select(a => new AnswerOption
          {
            QuestionID = question,
            AnswerID = a.AnswerID,
            OptionID = a.OptionID
          })
        });
      }

      exam.Answers = answers;
    }

    public Exam GetExam(int examId, bool loadIsCorrect)
    {
      Exam exam;
      int userID;
      int quizID;

      using (var cmd = _sessionManager.GetCurrentUnitOfWork().CreateSPCommand("QA_GetExam"))
      {        
        using (var dr = cmd.CreateAndAddParameter("ExamID", DbType.Int32, examId)
          .ExecuteReader())
        {          

          if (dr.Read())
          {
            exam = new Exam();
            exam.ExamID = dr.GetInt32("ExamID");
            exam.CurrentQuestionID = dr.GetInt32("QuestionID");
            exam.StartDate = dr.GetDateTime("StartDate");
            exam.EndDate = dr.GetNullableDateTime("EndDate");
            exam.Score = dr.GetNullableInt32("Score");

            quizID = dr.GetInt32("QuizID");
            userID = dr.GetInt32("UserAccountID");
          }
          else
          {
            return null;
          }

          dr.NextResult();
          this.ReadAnswers(exam, dr);
        }
      }

      exam.Quiz = this.GetQuiz(quizID, loadIsCorrect, true);
      exam.UserAccount = _securityDataAccess.GetUserAccount(userID);
      return exam;
    }

    public Answer GetOrCreateAnswer(Exam exam, Question question)
    {
      if (exam == null) throw new ArgumentNullException(nameof(exam));
      if (!exam.ExamID.HasValue) throw new ArgumentException("The exam must be saved before answers can be created");

      if (question == null) throw new ArgumentNullException(nameof(question));
      if (!question.QuestionID.HasValue) throw new ArgumentException("The question must be saved before answers can be created");

      using (var cmd = _sessionManager.GetCurrentUnitOfWork().CreateSPCommand("QA_GetOrCreateAnswer"))
      {
        using (var dr = cmd.CreateAndAddParameter("ExamID", DbType.Int32, exam.ExamID.Value)
          .CreateAndAddParameter("QuestionID", DbType.Int32, question.QuestionID.Value)
          .ExecuteReader())
        {
          var answer = new Answer();
          if (dr.Read())
          {

            answer.QuestionID = dr.GetInt32("QuestionID");
            answer.AnswerID = dr.GetInt32("AnswerID");
            answer.ExamID = dr.GetInt32("ExamID");
          }
          else
          {
            throw new DataException("No rows were returned by QA_GetOrCreateAnswer");
          }

          var options = new List<AnswerOption>();
          dr.NextResult();
          while (dr.Read())
          {
            var ao = new AnswerOption();
            ao.QuestionID = answer.QuestionID;
            ao.AnswerID = answer.AnswerID.Value;
            ao.OptionID = dr.GetInt32("OptionID");
          }
          answer.AnswerOptions = options;

          return answer;
        }          
      }      
    }

    public void ClearExistingOptionsFromAnswer(Answer answer)
    {
      if (answer == null) throw new ArgumentNullException(nameof(answer));
      if (!answer.AnswerID.HasValue) throw new ArgumentException("The answer must be saved before it can be cleared");

      using (var cmd = _sessionManager.GetCurrentUnitOfWork().CreateSPCommand("QA_ClearOptionsFromAnswer"))
      {
        cmd.CreateAndAddParameter("AnswerID", DbType.Int32, answer.AnswerID.Value)
          .ExecuteNonQuery();

        answer.AnswerOptions = new AnswerOption[] {};
      }
    }

    public void SaveOptionForAnswer(Answer answer, int option)
    {
      if (answer == null) throw new ArgumentNullException(nameof(answer));
      if (!answer.AnswerID.HasValue) throw new ArgumentException("The answer must be saved before options can be added");

      using (var cmd = _sessionManager.GetCurrentUnitOfWork().CreateSPCommand("QA_AddOptionToAnswer"))
      {
        cmd.CreateAndAddParameter("AnswerID", DbType.Int32, answer.AnswerID.Value)
          .CreateAndAddParameter("OptionID", DbType.Int32, option)
          .CreateAndAddParameter("QuestionID", DbType.Int32, answer.QuestionID)
          .ExecuteNonQuery();

        answer.AnswerOptions = answer.AnswerOptions.Union(new AnswerOption[]
        {
          new AnswerOption
          {
            AnswerID = answer.AnswerID.Value,
            OptionID = option,
            QuestionID = answer.QuestionID
          }
        });
      }
    }

    public void UpdateExam(Exam exam)
    {
      if (exam == null) throw new ArgumentNullException(nameof(exam));
      if (!exam.ExamID.HasValue) throw new ArgumentException("The exam must be saved before it can be updated");

      using (var cmd = _sessionManager.GetCurrentUnitOfWork().CreateSPCommand("QA_UpdateExam"))
      {
        cmd.CreateAndAddParameter("ExamID", DbType.Int32, exam.ExamID.Value)
          .CreateAndAddParameter("CurrentQuestionID", DbType.Int32, exam.CurrentQuestionID)
          .CreateAndAddParameter("EndDate", DbType.DateTime, exam.EndDate)
          .CreateAndAddParameter("Score", DbType.Int32, exam.Score)
          .ExecuteNonQuery();
      }
    }
  }
}
