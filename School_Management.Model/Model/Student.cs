using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace School_Management.Model
{
    [ConfigTableAttribute("student")]
    public partial class Student : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Class { get; set; }
        public int ParentId { get; set; }
        public virtual Parent Parent { get; set; }
    }
}
