using School_Management.BL;
using School_Management.Model;

namespace School_Management.Controllers
{
    public class ParentController : BaseController<Parent>
    {
        private readonly IParentBL _parentBL;

        public ParentController(IParentBL parentBL)
        {
            BL = parentBL;
        }
    }
}
