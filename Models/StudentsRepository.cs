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
            return ToList().Where(u => u.Code.ToLower() == code.ToLower()).FirstOrDefault() != null;
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