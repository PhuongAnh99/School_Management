using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Management.Model
{
    public class FilterItem
    {
        public string Prefix { get; set; }
        public string PropName { get; set; }
        public string Condition { get; set; }
        public object Value { get; set; }
    }
}
