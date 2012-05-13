using System.Web.Mvc;
using Castle.Core.Logging;
using Raven.Client;

namespace Rosterer.Frontend.Controllers
{
    public abstract class BaseController : Controller
    {
        public IDocumentSession RavenSession { get; private set; }
        public ILogger Logger { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if(filterContext.IsChildAction)
                //return;
            RavenSession = MvcApplication.Store.OpenSession();
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            using (RavenSession)
            {
                if (filterContext.Exception != null || RavenSession==null)
                    return;

                if (RavenSession != null)
                    RavenSession.SaveChanges();
            }
            base.OnActionExecuted(filterContext);
        }
    }
}