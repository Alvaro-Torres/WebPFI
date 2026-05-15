using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace Models
{
    public class StudentsRepository : Repository<Student>
    {
        public bool CodeExist(string code)
        {
            // Vérifier si le code existe déjà dans la liste des étudiants
            return ToList().Where(u => u.Code == code).FirstOrDefault() != null;
        }

        public override int Add(Student student)
        {
            while (CodeExist(student.Code))
            {
                student.Code = $"{DateTime.Now.Year}{new Random().Next(100000, 999999)}";
            }
            return base.Add(student);
        }
    }
}