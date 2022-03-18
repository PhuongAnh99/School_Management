using School_Management.BL;
using School_Management.Model;

namespace School_Management.Controllers
{
    public class StudentController : BaseController<Student>
    {
        private readonly IStudentBL _studentBL;

        public StudentController(IStudentBL studentBL)
        {
            BL = studentBL;
        }
    }
}
