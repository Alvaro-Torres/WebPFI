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

        public ActionResult GetStudentDetails(bool forceRefresh = false)
        {
            try
            {
                int studentId = (int)Session["CurrentStudentId"];
                Student student = DB.Students.Get(studentId);

                if (DB.Students.HasChanged || forceRefresh)
                {
                    return PartialView(student);
                }
                return null;
            }
            catch (System.Exception ex)
            {
                return Content("Erreur interne" + ex.Message, "text/html");
            }
        }

        public ActionResult Details(int id)
        {
            // Sauvegarder l'id de l'étudiant courant dans la session
            Session["CurrentStudentId"] = id;

            // Récupérer l'étudiant par son id
            Student student = DB.Students.Get(id);

            if (student != null)
            {
                // Sauvegarder le nom de l'étudiant courant dans la session
                Session["CurrentStudentName"] = student.FirstName + " " + student.LastName;
                return View(student);
            }

            // Si l'étudiant n'existe pas, retourner à la liste
            return RedirectToAction("Index");
        }

        public ActionResult Edit()
        {
            int id = Session["CurrentStudentId"] != null ? (int)Session["CurrentStudentId"] : 0;
            if (id != 0)
            {
                Student student = DB.Students.Get(id);
                if (student != null)
                    return View(student);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(Student student)
        {
            // L'id est récupéré de la session pour éviter les requêtes malicieuses
            int id = Session["CurrentStudentId"] != null ? (int)Session["CurrentStudentId"] : 0;

            // Vérifier que l'étudiant existe
            Student storedStudent = DB.Students.Get(id);
            if (storedStudent != null)
            {
                // Restaurer l'id et le code qui ne doivent pas être modifiés
                student.Id = id;
                student.Code = storedStudent.Code;

                DB.Students.Update(student);
                return RedirectToAction("Details/" + id);
            }
            return RedirectToAction("Index");
        }

    }
}