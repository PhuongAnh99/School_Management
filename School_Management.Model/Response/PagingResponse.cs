using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Management.Model
{
    public class PagingResponse
    {
        public object PageData { get; set; }
        public int Total { get; set; }
    }
}
