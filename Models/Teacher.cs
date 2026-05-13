using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Code { get; set; } = $"CLG-420-{new Random().Next(10000, 99999)}";
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public string Email { get; set; }
        public string Phone { get; set; }

        const string Avatars_Folder = @"/App_Assets/Teachers/";
        const string Default_Avatar = @"no_avatar.png";
        [Asset(Avatars_Folder, Default_Avatar)]
        public string Avatar { get; set; } = Avatars_Folder + Default_Avatar;


    }
}