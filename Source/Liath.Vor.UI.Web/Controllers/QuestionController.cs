﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Liath.Vor.Contracts.BusinessLogic;
using Liath.Vor.Contracts.DataAccess;
using Liath.Vor.Session;
using Liath.Vor.UI.Web.Models.Question;

namespace Liath.Vor.UI.Web.Controllers
{
  public class QuestionController : Controller
  {
    private readonly ISessionManager _sessionManager;
    private readonly IQuestionManager _questionManager;

    public QuestionController(ISessionManager sessionManager, IQuestionManager questionManager)
    {
      if (sessionManager == null) throw new ArgumentNullException(nameof(sessionManager));
      if (questionManager == null) throw new ArgumentNullException(nameof(questionManager));

      _sessionManager = sessionManager;
      _questionManager = questionManager;
    }

    [HttpGet]
    [ActionName("Exam")]
    public ActionResult GetExam(int id)
    {
      using (_sessionManager.CreateUnitOfWork())
      {
        var exam = _questionManager.GetExam(id);

        return Json(exam, JsonRequestBehavior.AllowGet);
      }
    }

    [HttpPost]
    [ActionName("Answer")]
    public ActionResult RecordAnswer(RecordAnswer answer)
    {
      using (_sessionManager.CreateUnitOfWork())
      {
        var nextQuestion = _questionManager.RecordAnswer(answer.ExamID,
          answer.QuestionID,
          answer.IsForwards,
          answer.Options);
        return Json(new
        {
          NextQuestionID = nextQuestion?.QuestionID
        });
      }
    }

    [HttpPost]
    [ActionName("SubmitExam")]
    public ActionResult RecordAnswer(SubmitExam answer)
    {
      using (_sessionManager.CreateUnitOfWork())
      {
        var examResults = _questionManager.SubmitExam(answer.ExamID);
        return Json(examResults);
      }
    }
  }
}