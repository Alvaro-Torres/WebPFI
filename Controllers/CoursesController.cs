using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using DAL;

namespace Controllers
{
    public class CoursesController : Controller
    {
        public ActionResult List()
        {
            return View();
        }

        // Action qui produit une vue partielle des cours
        // Destinée à être appelée par une requête AJAX
        public ActionResult GetCourses(bool forceRefresh = false)
        {
            IEnumerable<Course> result = null;
            // HasChanged est vrai si une modification a été apportée à un cours
            if (forceRefresh || DB.Courses.HasChanged)
            {
                result = DB.Courses.ToList().OrderBy(c => c.Code);
                return PartialView(result);
            }
            return null;
        }

        // Action qui produit une vue partielle des détails d'un cours
        // Destinée à être appelée par une requête AJAX
        public ActionResult GetCourseDetails(bool forceRefresh = false)
        {
            try
            {
                // Récupérer l'id du cours courant depuis la session
                int courseId = (int)Session["CurrentCourseId"];
                Course course = DB.Courses.Get(courseId);
                if (DB.Courses.HasChanged || forceRefresh)
                {
                    return PartialView(course);
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
            // Sauvegarder l'id du cours courant dans la session
            Session["CurrentCourseId"] = id;
            Course course = DB.Courses.Get(id);
            if (course != null)
            {
                // Sauvegarder le titre du cours courant dans la session
                Session["CurrentCourseName"] = course.Code + " " + course.Title;
                return View(course);
            }
            return RedirectToAction("List");
        }

        // L'id n'est pas fourni en paramètre, il est récupéré de la session
        // pour éviter les requêtes malicieuses
        public ActionResult Edit()
        {
            int id = Session["CurrentCourseId"] != null ? (int)Session["CurrentCourseId"] : 0;
            if (id != 0)
            {
                Course course = DB.Courses.Get(id);
                if (course != null)
                    return View(course);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(Course course)
        {
            // L'id est récupéré de la session pour éviter les requêtes malicieuses
            int id = Session["CurrentCourseId"] != null ? (int)Session["CurrentCourseId"] : 0;
            Course storedCourse = DB.Courses.Get(id);
            if (storedCourse != null)
            {
                // Restaurer l'id qui ne doit pas être modifié
                course.Id = id;
                DB.Courses.Update(course);
                return RedirectToAction("Details/" + id);
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            // Retourner un nouveau cours vide au formulaire
            return View(new Course());
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Course course)
        {
            // Ajouter le cours à la base de données
            DB.Courses.Add(course);
            return RedirectToAction("List");
        }

        public ActionResult Delete()
        {
            // L'id est récupéré de la session pour éviter les requêtes malicieuses
            int id = Session["CurrentCourseId"] != null ? (int)Session["CurrentCourseId"] : 0;
            if (id != 0)
            {
                DB.Courses.Delete(id);
            }
            return RedirectToAction("List");
        }
    }
}