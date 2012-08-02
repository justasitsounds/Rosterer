using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Castle.Windsor;

namespace Rosterer.Frontend.Plumbing
{
    public abstract class WindsorRoleProvider : RoleProvider 
    {
        private string providerId;

        public abstract IWindsorContainer GetContainer();

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
            providerId = config["providerId"];
            if (string.IsNullOrWhiteSpace(providerId))
                throw new Exception("Please configure the providerId from the role provider " + name);
        }

        private RoleProvider GetProvider()
        {
            try
            {
                var provider = GetContainer().Resolve<RoleProvider>(providerId, new Hashtable());
                if (provider == null)
                    throw new Exception(string.Format("Component '{0}' does not inherit RoleProvider", providerId));
                return provider;
            }
            catch (Exception e)
            {
                throw new Exception("Error resolving RoleProvider " + providerId, e);
            }
        }

        private T WithProvider<T>(Func<RoleProvider, T> f)
        {
            var provider = GetProvider();
            try
            {
                return f(provider);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }

        private void WithProvider(Action<RoleProvider> f)
        {
            var provider = GetProvider();
            try
            {
                f(provider);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return WithProvider(p => p.IsUserInRole(username, roleName));
        }

        public override string[] GetRolesForUser(string username)
        {
            return WithProvider(p => p.GetRolesForUser(username));
        }

        public override void CreateRole(string roleName)
        {          
            WithProvider(p => p.CreateRole(roleName));
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return WithProvider(p => p.DeleteRole(roleName, throwOnPopulatedRole));
        }

        public override bool RoleExists(string roleName)
        {
            return WithProvider(p => p.RoleExists(roleName));
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            WithProvider(p => p.AddUsersToRoles(usernames, roleNames));
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            WithProvider(p => p.RemoveUsersFromRoles(usernames, roleNames));
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return WithProvider(p => p.GetUsersInRole(roleName));
        }

        public override string[] GetAllRoles()
        {
            return WithProvider(p => p.GetAllRoles());
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return WithProvider(p => p.FindUsersInRole(roleName, usernameToMatch));
        }

        public override string ApplicationName
        {
            get { return WithProvider(p => p.ApplicationName); }
            set { WithProvider(p => p.ApplicationName = value); }
        }
    }

    public class WebWindsorRoleProvider : WindsorRoleProvider
    {
        public override IWindsorContainer GetContainer()
        {
            var context = HttpContext.Current;
            if (context == null)
                throw new Exception("No HttpContext");
            var accessor = context.ApplicationInstance as IContainerAccessor;
            if (accessor == null)
                throw new Exception("The global HttpApplication instance needs to implement " + typeof(IContainerAccessor).FullName);
            if (accessor.Container == null)
                throw new Exception("HttpApplication has no container initialized");
            return accessor.Container;
        }
    }
}