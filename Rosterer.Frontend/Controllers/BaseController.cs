using System.Web.Mvc;
using Castle.Core.Logging;
using Raven.Client;
using Rosterer.Domain;

namespace Rosterer.Frontend.Controllers
{
    public abstract class BaseController : Controller
    {
        public IDocumentSession RavenSession { get; set; }
        public ISessionState CurrentEditSession { get; set; } 
       
    }
}