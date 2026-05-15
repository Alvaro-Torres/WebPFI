using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Registration : Record
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Year { get; set; }
    }
}