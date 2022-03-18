using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School_Management.Model;

namespace School_Management.BL
{
    public class TeacherBL : BaseBL<Teacher>, ITeacherBL
    {
        public TeacherBL(SchoolContext dbcontext) : base(dbcontext)
        {

        }
    }
}
