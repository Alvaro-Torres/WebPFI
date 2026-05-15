using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using DAL;

namespace Controllers
{
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
            // HasChanged est vrai si une modification a été apportée à un enseignant
            if (forceRefresh || DB.Teachers.HasChanged)
            {
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
        public ActionResult Edit()
        {
            int id = Session["CurrentTeacherId"] != null ? (int)Session["CurrentTeacherId"] : 0;
            if (id != 0)
            {
                Teacher teacher = DB.Teachers.Get(id);
                if (teacher != null)
                    return View(teacher);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(Teacher teacher)
        {
            // L'id est récupéré de la session pour éviter les requêtes malicieuses
            int id = Session["CurrentTeacherId"] != null ? (int)Session["CurrentTeacherId"] : 0;
            Teacher storedTeacher = DB.Teachers.Get(id);
            if (storedTeacher != null)
            {
                // Restaurer l'id et le code qui ne doivent pas être modifiés
                teacher.Id = id;
                teacher.Code = storedTeacher.Code;
                DB.Teachers.Update(teacher);
                return RedirectToAction("Details/" + id);
            }
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            // Retourner un nouvel enseignant vide au formulaire
            return View(new Teacher());
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Teacher teacher)
        {
            // Ajouter l'enseignant à la base de données
            DB.Teachers.Add(teacher);
            return RedirectToAction("List");
        }

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
    }
}