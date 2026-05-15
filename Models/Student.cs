using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class Student : Record
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Code { get; set; } = $"{DateTime.Now.Year}{new Random().Next(100000, 1000000)}";
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public string Email { get; set; }
        public string Phone { get; set; }

        // Nom complet calculé
        [JsonIgnore]
        public string FullName => LastName + " " + FirstName;

        // Code calculé pour affichage
        [JsonIgnore]
        public string Caption => Code + " " + LastName + " " + FirstName;

        // Toutes les inscriptions de l'étudiant
        [JsonIgnore]
        public List<Registration> Registrations => DB.Registrations.ToList()
            .Where(r => r.StudentId == Id).ToList();

        // Inscriptions de la prochaine session
        [JsonIgnore]
        public List<Registration> NextSessionRegistrations => DB.Registrations.ToList()
            .Where(r => r.StudentId == Id && r.IsNextSession).ToList();

        // Liste des cours pour SelectList
        [JsonIgnore]
        public List<Course> Courses
        {
            get
            {
                var courses = new List<Course>();
                foreach (var registration in Registrations.OrderBy(r => r.Course.Code))
                {
                    courses.Add(registration.Course);
                }
                return courses;
            }
        }

        // Liste des cours de la prochaine session pour SelectList
        [JsonIgnore]
        public List<Course> NextSessionCourses
        {
            get
            {
                var courses = new List<Course>();
                foreach (var registration in NextSessionRegistrations.OrderBy(r => r.Course.Code))
                {
                    courses.Add(registration.Course);
                }
                return courses;
            }
        }

        // Supprimer toutes les inscriptions
        public void DeleteAllRegistrations()
        {
            foreach (var registration in Registrations)
                DB.Registrations.Delete(registration.Id);
        }

        // Supprimer les inscriptions de la prochaine session
        public void DeleteNextSessionRegistrations()
        {
            foreach (var registration in NextSessionRegistrations)
                DB.Registrations.Delete(registration.Id);
        }

        // Mettre à jour les inscriptions de la prochaine session
        public void UpdateRegistrations(List<int> selectedCoursesId)
        {
            DeleteNextSessionRegistrations();
            if (selectedCoursesId != null)
                foreach (var courseId in selectedCoursesId)
                    DB.Registrations.Add(new Registration { StudentId = Id, CourseId = courseId });
        }
    }
}