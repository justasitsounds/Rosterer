using System;
using System.Collections.Generic;
using Castle.Windsor;

namespace Rosterer.Domain
{
    /// <summary>
    /// http://www.udidahan.com/2009/06/14/domain-events-salvation/
    /// </summary>
    public static class DomainEvents
    {
        [ThreadStatic] //so that each thread has its own callbacks
            private static List<Delegate> _actions;

        public static IWindsorContainer Container { get; set; } //as before

        //Registers a callback for the given domain event
        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (_actions == null)
                _actions = new List<Delegate>();
            _actions.Add(callback);
        }

        //Clears callbacks passed to Register on the current thread
        public static void ClearCallbacks()
        {
            _actions = null;
        }

        //Raises the given domain event
        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if (Container != null)
                foreach (var handler in Container.ResolveAll<IHandle<T>>())
                    handler.Handle(args);

            if (_actions != null)
                foreach (Delegate action in _actions)
                    if (action is Action<T>)
                        ((Action<T>) action)(args);
        }
    }

    public interface IDomainEvent
    {
    }

    public interface IHandle<T> where T : IDomainEvent
    {
        void Handle(T args);
    }
}