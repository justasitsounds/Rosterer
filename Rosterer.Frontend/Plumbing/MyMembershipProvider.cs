using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Security;
using Raven.Client;
using Raven.Client.Linq;
using Rosterer.Domain;
using Rosterer.Domain.Entities;
using Rosterer.Frontend.Models;

namespace Rosterer.Frontend.Plumbing
{
    public class MyMembershipProvider : MembershipProvider
    {
        public IDocumentStore RavenStore { get; private set; }

        public string ProviderName { get { return "customProvider"; } }

        public MyMembershipProvider()
        {
            RavenStore = MvcApplication.Store;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            var names = username.Split(new[]{" "},StringSplitOptions.None);
            return CreateUser(names[0], names[1], password, email, passwordQuestion, passwordAnswer, isApproved,
                              providerUserKey, out status);
        }

        public MembershipUser CreateUser(string firstname,string lastname, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            status = MembershipCreateStatus.ProviderError;
            return null;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return true;
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return true;
        }

        public override string ResetPassword(string username, string answer)
        {
            return null;
        }

        public override void UpdateUser(MembershipUser user)
        {
            
        }

        public override bool ValidateUser(string username, string password)
        {
            using(var session = RavenStore.OpenSession())
            {
                
                var candidate = session.Query<User>().SingleOrDefault(u => u.EmailAddress == username);
                return candidate != null && BCrypt.CheckPassword(password, candidate.PasswordHash);
            }
        }

        public override bool UnlockUser(string userName)
        {
            return true;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            using (var session = RavenStore.OpenSession())
            {
                var user = session.Load<User>((string) providerUserKey);
                return AutoMapper.Mapper.Map<User, RosterMembershipUser>(user);
            }
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return null;
        }

        public override string GetUserNameByEmail(string email)
        {
            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return true;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return PagedMembershipUserCollection(pageIndex, pageSize, out totalRecords, u => !string.IsNullOrEmpty(u.Id));
        }

        private MembershipUserCollection PagedMembershipUserCollection(int pageIndex, int pageSize, out int totalRecords, Expression<Func<User, bool>> expression)
        {
            using (var session = RavenStore.OpenSession())
            {
                RavenQueryStatistics stats;
                var users = session.Query<User>()
                    .Statistics(out stats)
                    .Where(expression)
                    .Skip(pageIndex*pageSize)
                    .Take(pageSize)
                    .ToArray();
                totalRecords = stats.TotalResults;
                var rosterUsers = AutoMapper.Mapper.Map<User[], RosterMembershipUser[]>(users);
                var collection = new MembershipUserCollection();
                foreach (var rosterMembershipUser in rosterUsers)
                {
                    collection.Add(rosterMembershipUser);
                }
                return collection;
            }
        }

        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return PagedMembershipUserCollection(pageIndex, pageSize, out totalRecords,
                                                 u => u.EmailAddress == emailToMatch);
        }

        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        public override bool EnablePasswordReset
        {
            get { return false; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 10; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 10; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return null; }
        }
    }
}