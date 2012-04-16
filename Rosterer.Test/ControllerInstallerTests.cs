using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.Windsor;
using Rosterer.Frontend.Controllers;
using Rosterer.Frontend.Installers;
using Xunit;

namespace Rosterer.Test
{
    public class ControllerInstallerTests
    {
        private readonly IWindsorContainer containerWithControllers;

        public ControllerInstallerTests()
        {
            containerWithControllers = new WindsorContainer()
                .Install(new ControllerInstaller());
        }

        [Fact]
        public void AllControllersImplementIController()
        {
            var allHandlers = GetAllHandlers(containerWithControllers);
            var controllerHandlers = GetHandlersFor(typeof(IController),containerWithControllers);

            Assert.NotEmpty(allHandlers);
            Assert.Equal(allHandlers, controllerHandlers);
        }

        [Fact]
        public void AllControllersAreRegistered()
        {
            var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Is<IController>());
            var registeredControllers = GetImplementationTypesFor(typeof(IController), containerWithControllers);
            Assert.Equal(allControllers, registeredControllers);
        }

        [Fact]
        public void AllAndOnlyControllersHaveControllersSuffix()
        {
            var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Name.EndsWith("Controller"));
            var registeredControllers = GetImplementationTypesFor(typeof(IController), containerWithControllers);
            Assert.Equal(allControllers, registeredControllers);
        }

        [Fact]
        public void AllAndOnlyControllersLiveInControllersNamespace()
        {
            var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Namespace.Contains("Controllers"));
            var registeredControllers = GetImplementationTypesFor(typeof(IController), containerWithControllers);
            Assert.Equal(allControllers, registeredControllers);
        }

        [Fact]
        public void AllControllersAreTransient()
        {
            var nonTransientControllers = GetHandlersFor(typeof(IController), containerWithControllers)
                .Where(controller => controller.ComponentModel.LifestyleType != LifestyleType.Transient)
                .ToArray();

            Assert.Empty(nonTransientControllers);
        }

        [Fact]
        public void AllControllersExposeThemselvesAsService()
        {
            var controllersWithWrongName = GetHandlersFor(typeof(IController), containerWithControllers)
                .Where(controller => controller.ComponentModel.Services.Single() != controller.ComponentModel.Implementation)
                .ToArray();

            Assert.Empty(controllersWithWrongName);
        }

        private Type[] GetImplementationTypesFor(Type type, IWindsorContainer container)
        {
            return GetHandlersFor(type, container)
                .Select(h => h.ComponentModel.Implementation)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        private Type[] GetPublicClassesFromApplicationAssembly(Predicate<Type> where)
        {
            return typeof (HomeController).Assembly.GetExportedTypes()
                .Where(t => t.IsClass)
                .Where(t => t.IsAbstract == false)
                .Where(where.Invoke)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        private IHandler[] GetAllHandlers(IWindsorContainer windsorContainer)
        {
            return GetHandlersFor(typeof (object), windsorContainer);
        }

        private IHandler[] GetHandlersFor(Type controllerType, IWindsorContainer windsorContainer)
        {
            return windsorContainer.Kernel.GetAssignableHandlers(controllerType);
        }
    }
}
