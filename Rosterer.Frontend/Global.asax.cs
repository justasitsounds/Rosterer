﻿using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Rosterer.Domain;
using Rosterer.Domain.Entities;
using Rosterer.Domain.Events;
using Rosterer.Frontend.ObjectMappers;
using Rosterer.Frontend.Plumbing;

namespace Rosterer.Frontend
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication, IContainerAccessor
    {
        
        private static IWindsorContainer container;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("{*js}", new { favicon = @"(.*/)?.js(/.*)?" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );
            routes.MapRoute(
                "Week",
                "Week/{year}/{week}",
                new { controller = "Week", action="Index"}
                );
        }

        protected void Application_Start()
        {
            AutoMapperConfiguration.Configure();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            
            BootstrapContainer();

            container.Register(Component.For<MyMembershipProvider>()
                .LifeStyle.Transient
                .Named("myProvider"));

            DomainEvents.Container = container;
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            var logEntry = new LogEntry
            {
                Date = DateTime.Now,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
            };

            using (var datacontext = container.Resolve<IDocumentStore>().OpenSession())
            {
                datacontext.Store(logEntry);
                datacontext.SaveChanges();
            }                               
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
            if (container == null)
            {
                container = new WindsorContainer()
                    .Install(FromAssembly.This());
                var controllerFactory = new WindsorControllerFactory(container);
                ControllerBuilder.Current.SetControllerFactory(controllerFactory);
                container.Register(
                    Component.For<HttpSessionStateBase>()
                        .LifeStyle.PerWebRequest
                        .UsingFactoryMethod(() => new HttpSessionStateWrapper(HttpContext.Current.Session))
                    );

                container.Register(AllTypes.FromAssembly(Assembly.GetAssembly(typeof (CalendarBooking)))
                                       .BasedOn(typeof (IHandle<>))
                                       .WithService.AllInterfaces().LifestylePerWebRequest());
            }



        }
    }
}