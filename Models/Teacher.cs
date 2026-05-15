using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class Teacher : Record
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Code { get; set; } = $"CLG-420-{new Random().Next(10000, 99999)}";
        public DateTime StartDate { get; set; } = DateTime.Now;
        public string Email { get; set; }
        public string Phone { get; set; }

        const string Avatars_Folder = @"/App_Assets/Teachers/";
        const string Default_Avatar = @"no_avatar.png";
        [Asset(Avatars_Folder, Default_Avatar)]
        public string Avatar { get; set; } = Avatars_Folder + Default_Avatar;

        // Nom complet calculé
        [JsonIgnore]
        public string FullName => LastName + " " + FirstName;

        // Code calculé pour affichage
        [JsonIgnore]
        public string Caption => Code + " " + LastName + " " + FirstName;

        // Toutes les allocations de l'enseignant
        [JsonIgnore]
        public List<Allocation> Allocations => DB.Allocations.ToList()
            .Where(a => a.TeacherId == Id).ToList();

        // Allocations de la prochaine session
        [JsonIgnore]
        public List<Allocation> NextSessionAllocations => DB.Allocations.ToList()
            .Where(a => a.TeacherId == Id && a.IsNextSession).ToList();

        // Liste des cours pour SelectList
        [JsonIgnore]
        public List<Course> Courses
        {
            get
            {
                var courses = new List<Course>();
                foreach (var allocation in Allocations.OrderBy(a => a.Course.Code))
                {
                    courses.Add(allocation.Course);
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
                foreach (var allocation in NextSessionAllocations.OrderBy(a => a.Course.Code))
                {
                    courses.Add(allocation.Course);
                }
                return courses;
            }
        }

        // Supprimer toutes les allocations
        public void DeleteAllAllocations()
        {
            foreach (var allocation in Allocations)
                DB.Allocations.Delete(allocation.Id);
        }

        // Supprimer les allocations de la prochaine session
        public void DeleteNextSessionAllocations()
        {
            foreach (var allocation in NextSessionAllocations)
                DB.Allocations.Delete(allocation.Id);
        }

        // Mettre à jour les allocations de la prochaine session
        public void UpdateAllocations(List<int> selectedCoursesId)
        {
            DeleteNextSessionAllocations();
            if (selectedCoursesId != null)
                foreach (var courseId in selectedCoursesId)
                    DB.Allocations.Add(new Allocation { TeacherId = Id, CourseId = courseId });
        }
    }
}