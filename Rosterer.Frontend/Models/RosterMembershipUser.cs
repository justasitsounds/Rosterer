using System.Drawing;
using System.Web.Security;

namespace Rosterer.Frontend.Models
{
    public class RosterMembershipUser : MembershipUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Color Color { get; set; }
    }
}