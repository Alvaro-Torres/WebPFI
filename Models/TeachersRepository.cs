using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class TeachersRepository : Repository<Teacher>
    {
        public bool CodeExist(string code)
        {
            return ToList().Where(u => u.Code.ToLower() == code.ToLower()).FirstOrDefault() != null;
        }

        public override int Add(Teacher teacher)
        {
            while (CodeExist(teacher.Code))
            {
                teacher.Code = $"CLG-420-{new Random().Next(10000, 99999)}";
            }
            return base.Add(teacher);
        }
    }
}