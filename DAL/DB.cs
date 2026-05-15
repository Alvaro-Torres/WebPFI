//using EmailHandling;
using Antlr.Runtime;
using Controllers;
using EmailHandling;
using Models;
using System;

//using PhotosManager.Models;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;

namespace DAL
{
    public sealed class DB
    {
        #region singleton setup
        private static readonly DB instance = new DB();
        public static DB Instance { get { return instance; } }
        #endregion

        static public UsersRepository Users { get; set; }
            = new UsersRepository();

        public static StudentsRepository Students { get; set; }
            = new StudentsRepository();

        public static TeachersRepository Teachers { get; set; }
            = new TeachersRepository();

        public static Repository<Allocation> Allocations { get; set; }
             = new Repository<Allocation>();

        public static Repository<Registration> Registrations { get; set; }
            = new Repository<Registration>();

        public static Repository<Course> Courses { get; set; }
            = new Repository<Course>();

        static public NotificationsRepository Notifications { get; set; }
            = new NotificationsRepository();

        static public LoginsRepository Logins { get; set; }
           = new LoginsRepository();

        static public EventsRepository Events { get; set; }
            = new EventsRepository();

        static public Repository<RenewPasswordCommand> RenewPasswordCommands { get; set; }
          = new Repository<RenewPasswordCommand>();

        static public Repository<UnverifiedEmail> UnverifiedEmails { get; set; }
            = new Repository<UnverifiedEmail>();
    }
}