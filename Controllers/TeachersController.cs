using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Controllers.AccessControl;

namespace Controllers
{
    [UserAccess(Models.Access.View)]
    public class TeachersController : Controller
    {
        public ActionResult List()
        {
            return View();
        }

        // Action qui produit une vue partielle des enseignants
        // Destinée à être appelée par une requête AJAX
        public ActionResult GetTeachers(bool forceRefresh = false)
        {
            IEnumerable<Teacher> result = null;
            if (forceRefresh || DB.Teachers.HasChanged)
            {
                bool search = Session["Search"] != null ? (bool)Session["Search"] : false;
                string searchString = Session["SearchString"] != null ? (string)Session["SearchString"] : "";

                // Appliquer la recherche si elle est activée et que la chaîne de recherche n'est pas vide
                if (search && !string.IsNullOrEmpty(searchString))
                    result = DB.Teachers.ToList()
                               .Where(t => t.LastName.ToLower().Contains(searchString) ||
                                           t.FirstName.ToLower().Contains(searchString))
                               .OrderBy(t => t.LastName);
                else
                    result = DB.Teachers.ToList().OrderBy(t => t.LastName);

                return PartialView(result);
            }
            return null;
        }

        // Action qui produit une vue partielle des détails d'un enseignant
        // Destinée à être appelée par une requête AJAX
        public ActionResult GetTeacherDetails(bool forceRefresh = false)
        {
            try
            {
                // Récupérer l'id de l'enseignant courant depuis la session
                int teacherId = (int)Session["CurrentTeacherId"];
                Teacher teacher = DB.Teachers.Get(teacherId);
                if (DB.Teachers.HasChanged || forceRefresh)
                {
                    return PartialView(teacher);
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
            // Sauvegarder l'id de l'enseignant courant dans la session
            Session["CurrentTeacherId"] = id;
            Teacher teacher = DB.Teachers.Get(id);
            if (teacher != null)
            {
                // Sauvegarder le nom de l'enseignant courant dans la session
                Session["CurrentTeacherName"] = teacher.FirstName + " " + teacher.LastName;
                return View(teacher);
            }
            return RedirectToAction("List");
        }

        // L'id n'est pas fourni en paramètre, il est récupéré de la session
        // pour éviter les requêtes malicieuses
        [UserAccess(Models.Access.Write)]
        public ActionResult Edit()
        {
            int id = Session["CurrentTeacherId"] != null ? (int)Session["CurrentTeacherId"] : 0;
            if (id != 0)
            {
                Teacher teacher = DB.Teachers.Get(id);
                if (teacher != null)
                {
                    /* Passer les listes de cours pour le widget de sélection */
                    ViewBag.Allocations = new SelectList(
                        teacher.NextSessionCourses
                        .Select(c => new { c.Id, Display = c.Code + " " + c.Title }),
                        "Id", "Display");

                    ViewBag.Courses = new SelectList(
                        DB.Courses.ToList().OrderBy(c => c.Code)
                        .Select(c => new { c.Id, Display = c.Code + " " + c.Title }),
                        "Id", "Display");

                    return View(teacher);
                }
            }
            return RedirectToAction("List");
        }

        [UserAccess(Models.Access.Write)]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(Teacher teacher, List<int> selectedCoursesId)
        {
            int id = Session["CurrentTeacherId"] != null ? (int)Session["CurrentTeacherId"] : 0;
            Teacher storedTeacher = DB.Teachers.Get(id);
            if (storedTeacher != null)
            {
                teacher.Id = id;
                teacher.Code = storedTeacher.Code;
                teacher.UpdateAllocations(selectedCoursesId);
                DB.Teachers.Update(teacher);
                return RedirectToAction("Details/" + id);
            }
            return RedirectToAction("List");
        }
        [UserAccess(Models.Access.Write)]

        public ActionResult Create()
        {
            // Retourner un nouvel enseignant vide au formulaire
            return View(new Teacher());
        }

        [UserAccess(Models.Access.Write)]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Teacher teacher)
        {
            // Ajouter l'enseignant à la base de données
            DB.Teachers.Add(teacher);
            return RedirectToAction("List");
        }

        [UserAccess(Models.Access.Write)]
        public ActionResult Delete()
        {
            // L'id est récupéré de la session pour éviter les requêtes malicieuses
            int id = Session["CurrentTeacherId"] != null ? (int)Session["CurrentTeacherId"] : 0;
            if (id != 0)
            {
                DB.Teachers.Delete(id);
            }
            return RedirectToAction("List");
        }

        public ActionResult ToggleSearch()
        {
            // Activer/désactiver la recherche
            if (Session["Search"] == null) Session["Search"] = false;
            Session["Search"] = !(bool)Session["Search"];
            return RedirectToAction("List");
        }

        public ActionResult SetSearchString(string value)
        {
            // Sauvegarder la chaîne de recherche en minuscules
            Session["SearchString"] = value.ToLower();
            return RedirectToAction("List");
        }
    }
}