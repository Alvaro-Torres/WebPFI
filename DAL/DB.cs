//using EmailHandling;
using Antlr.Runtime;
using Controllers;
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

        public static Repository<Student> Students { get; set; }
            = new Repository<Student>();

        //  static public NotificationsRepository Notifications { get; set; }
        //      = new NotificationsRepository();

        // static public LoginsRepository Logins { get; set; }
        //    = new LoginsRepository();

        // static public EventsRepository Events { get; set; }
        //     = new EventsRepository();

        // static public Repository<RenewPasswordCommand> RenewPasswordCommands { get; set; }
        //   = new Repository<RenewPasswordCommand>();


    }
}