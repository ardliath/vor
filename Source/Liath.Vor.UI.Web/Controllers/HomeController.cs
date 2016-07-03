using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Liath.Vor.Contracts.BusinessLogic;
using Liath.Vor.Session;

namespace Liath.Vor.UI.Web.Controllers
{
    public class HomeController : Controller
    {
      private readonly ISessionManager _sessionManager;
      private readonly ISecurityManager _securityManager;

      public HomeController(ISessionManager sessionManager, ISecurityManager securityManager)
      {
        if (sessionManager == null) throw new ArgumentNullException(nameof(sessionManager));
        if (securityManager == null) throw new ArgumentNullException(nameof(securityManager));

        _sessionManager = sessionManager;
        _securityManager = securityManager;
      }

      // GET: Home
      public ActionResult Index()
      {
        using (_sessionManager.CreateUnitOfWork())
        {
          _securityManager.EnsureUserAccountExists();        
          return View();
        }
      }
    }
}