using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Liath.Vor.Contracts.DataAccess;
using Liath.Vor.Session;

namespace Liath.Vor.UI.Web.Controllers
{
  public class QuestionController : Controller
  {
    private readonly ISessionManager _sessionManager;
    private readonly IQuestionDataAccess _questionDataAccess;

    public QuestionController(ISessionManager sessionManager, IQuestionDataAccess questionDataAccess)
    {
      if (sessionManager == null) throw new ArgumentNullException(nameof(sessionManager));
      if (questionDataAccess == null) throw new ArgumentNullException(nameof(questionDataAccess));
      _sessionManager = sessionManager;
      _questionDataAccess = questionDataAccess;
    }

    [HttpGet]
    [ActionName("Quiz")]
    public ActionResult GetQuiz(int id)
    {
      using (_sessionManager.CreateUnitOfWork())
      {
        var quiz = _questionDataAccess.GetQuiz(id, false, false);
        if (quiz == null) return HttpNotFound();

        return Json(quiz, JsonRequestBehavior.AllowGet);
      }
    }
  }
}