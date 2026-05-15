using DAL;
using Newtonsoft.Json;
using System.Linq;

namespace Models
{
    public class Registration : Record
    {
        public Registration()
        {
            // Initialiser l'année avec la prochaine session
            Year = NextSession.Year;
        }

        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Year { get; set; }

        // Propriétés calculées ignorées par JSON
        [JsonIgnore]
        public Course Course => DB.Courses.Get(CourseId);

        [JsonIgnore]
        public Student Student => DB.Students.Get(StudentId);

        // Vrai si l'inscription est pour la prochaine session
        [JsonIgnore]
        public bool IsNextSession => Year == NextSession.Year && NextSession.ValidSessions.Contains(Course.Session);
    }
}