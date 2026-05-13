using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using DAL;

namespace Controllers
{
    public class StudentsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // This action produce a partial view of students
        // It is meant to be called by an AJAX request (from client script)
        public ActionResult GetStudents(bool forceRefresh = false)
        {
            IEnumerable<Student> result = null;
            if (forceRefresh || DB.Students.HasChanged)
            {
                result = DB.Students.ToList().OrderBy(c => c.LastName);
                return PartialView(result);
            }
            return null;
        }
    }
}