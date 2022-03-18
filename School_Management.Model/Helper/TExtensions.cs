using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace School_Management.Model
{
    public static class TExtension
    {
        /// <summary>
        /// Hàm thực hiện set giá trị cho property
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetValue(this object source, string propertyName, object value)
        {
            var prop = source.GetType().GetProperty(propertyName, BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (prop != null)
            {
                var type = prop.PropertyType;
                if (value != DBNull.Value && prop.CanWrite)
                {
                    if (value != null)
                        prop.SetValue(source, Convert.ChangeType(value, Nullable.GetUnderlyingType(type) ?? type), null);
                    else
                        prop.SetValue(source, null, null);
                }
            }
        }
    }
}
