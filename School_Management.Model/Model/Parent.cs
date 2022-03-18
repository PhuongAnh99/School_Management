﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Management.Model
{
    [ConfigTableAttribute("parent")]
    public partial class Parent : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public virtual List<Student> Students { get; set; }
    }
}
