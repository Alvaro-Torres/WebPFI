using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{
    public class StudentsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}