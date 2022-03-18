using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Management.Model
{
    public class BaseModel
    {
        /// <summary>
        /// Hàm lấy tên table trong db của model
        /// </summary>
        /// <returns></returns>
        public string GetTableName()
        {
            var config = (ConfigTableAttribute)this.GetType().GetCustomAttributes(typeof(ConfigTableAttribute), false).FirstOrDefault();
            return config?.TableName;
        }
    }
}
