using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Student : Record
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Code { get; set; } = $"{DateTime.Now.Year}{new Random().Next(100000, 999999)}";
        public DateTime BirthDate { get; set; } = DateTime.Now ;
        public string Email { get; set; }
        public string Phone { get; set; }

    }

}