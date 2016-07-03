using System;
using System.Collections.Generic;
using System.Data;
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
    public QuestionDataAccess(ISessionManager sessionManager)
    {
      if (sessionManager == null) throw new ArgumentNullException(nameof(sessionManager));
      _sessionManager = sessionManager;
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
  }
}
