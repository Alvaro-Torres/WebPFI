using DAL;
using Newtonsoft.Json;
using System.Linq;

namespace Models
{
    public class Allocation : Record
    {
        public Allocation()
        {
            // Initialiser l'année avec la prochaine session
            Year = NextSession.Year;
        }

        public int TeacherId { get; set; }
        public int CourseId { get; set; }
        public int Year { get; set; }

        // Propriétés calculées ignorées par JSON
        [JsonIgnore]
        public Course Course => DB.Courses.Get(CourseId);

        [JsonIgnore]
        public Teacher Teacher => DB.Teachers.Get(TeacherId);

        // Vrai si l'allocation est pour la prochaine session
        [JsonIgnore]
        public bool IsNextSession => Year == NextSession.Year && NextSession.ValidSessions.Contains(Course.Session);
    }
}