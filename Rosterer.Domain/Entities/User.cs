using System.Drawing;

namespace Rosterer.Domain.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public Color DisplayColour { get; set; }
        public string PasswordHash { get; set; }

        public User()
        {
            
        }
    }
}