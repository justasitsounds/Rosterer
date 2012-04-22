using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Raven.Abstractions.Data;
using Raven.Client.Document;
using Rosterer.Frontend.ObjectMappers;
using Rosterer.Frontend.Plumbing;

namespace Rosterer.Frontend
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication, IContainerAccessor
    {
        public static DocumentStore Store { get; set; }
        private static IWindsorContainer container;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );
        }

        protected void Application_Start()
        {
            AutoMapperConfiguration.Configure();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ConnectionStringParser<RavenConnectionStringOptions> parser =
                ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
            parser.Parse();

            Store = new DocumentStore
                        {
                            ApiKey = parser.ConnectionStringOptions.ApiKey,
                            Url = parser.ConnectionStringOptions.Url,
                        };

            Store.Initialize();
            Store.Conventions.IdentityPartsSeparator = "-";

            BootstrapContainer();

            container.Register(Component.For<MyMembershipProvider>()
                .LifeStyle.Transient
                .Named("myProvider"));
        }

        protected void Application_End()
        {
            container.Dispose();
        }

        public IWindsorContainer Container
        {
            get { return container; }
        }

        private static void BootstrapContainer()
        {
            container = new WindsorContainer()
                .Install(FromAssembly.This());
            var controllerFactory = new WindsorControllerFactory(container);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}