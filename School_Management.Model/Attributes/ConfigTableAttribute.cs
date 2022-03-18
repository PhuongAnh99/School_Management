using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Management.Model
{
    class ConfigTableAttribute : Attribute
    {
        public string TableName { get; set; }
        public ConfigTableAttribute(string tableName)
        {
            this.TableName = tableName;
        }
    }
}
