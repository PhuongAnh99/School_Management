using School_Management.BL;
using School_Management.Model;

namespace School_Management.Controllers
{
    public class TeacherController : BaseController<Teacher>
    {
        private readonly ITeacherBL _teacherBL;

        public TeacherController(ITeacherBL teacherBL)
        {
            BL = teacherBL;
        }
    }
}
