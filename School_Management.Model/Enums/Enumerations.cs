using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Management.Model
{
    public static class Enumerations
    {
        /// <summary>
        /// Mã Code response
        /// </summary>
        public enum Code : int
        {
            Success = 0,
            ErrorCRUD = 1,
            Exception = 1000
        }
    }
}
