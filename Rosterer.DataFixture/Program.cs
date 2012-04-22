using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Abstractions.Data;
using Raven.Client.Document;
using Rosterer.Domain;

namespace Rosterer.DataFixture
{
    public class Program
    {
        public static DocumentStore Store { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("* Rosterer Data Import                     *");
            Console.WriteLine("********************************************");
            Console.WriteLine("* Init RavenDB                             *");
            InitRavenDb();
            Console.WriteLine("* SetUpUsers                               *");
            SetUpUsers();
            Console.WriteLine("********************************************");
            Console.WriteLine(" ... Press any key to exit ");
            Console.ReadKey();

        }

        private static void SetUpUsers()
        {
            using (var session = Store.OpenSession())
            {
                var userCount = session.Query<User>().Count();
                var salt = BCrypt.GenerateSalt(12);
                var staff = new List<User>()
                                    { 
                                        new User(){
                                            DisplayColour = System.Drawing.ColorTranslator.FromHtml("#cc9966"),
                                            EmailAddress = "angie_sceats@hotmail.com",
                                            FirstName = "Angie",
                                            LastName = "Sceats",
                                            
                                            PasswordHash = BCrypt.HashPassword("chelsea",salt)
                                        }
                                    };
                if (userCount == 0)
                {
                    foreach (var staffMember in staff)
                    {
                        session.Store(staffMember);
                    }

                    session.SaveChanges();
                    return;
                }
                Console.WriteLine(string.Format("* - found {0} users    skipping import            *", userCount));


            }

           
        }

        private static void InitRavenDb()
        {
            ConnectionStringParser<RavenConnectionStringOptions> parser =
                ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
            parser.Parse();

            Store = new DocumentStore
                        {
                            ApiKey = parser.ConnectionStringOptions.ApiKey,
                            Url = parser.ConnectionStringOptions.Url,
                        };

            Store.Initialize();
        }
    }
}
