using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Management.Model
{
    public static class TConvert
    {
        /// <summary>
        /// Hàm thực hiện parse .NET object sang string Json
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ignoreNull"></param>
        /// <returns></returns>
        public static string Serialize(object source, bool ignoreNull = false)
        {
            var setting = GetSetting();

            if (ignoreNull)
                setting.NullValueHandling = NullValueHandling.Ignore;

            return JsonConvert.SerializeObject(source, setting);
        }

        /// <summary>
        /// Hàm thực hiện parse từ string Json sang .NET object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, GetSetting());
            }
            catch (Exception e)
            {
                if (typeof(T) == typeof(string))
                    return (T)((object)json);
                throw e;
            }
        }

        /// <summary>
        /// Hàm thực hiện map 1 object động sang 1 type đã được chỉ định
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <param name="ignoreNull"></param>
        /// <returns></returns>
        public static object SerializeObjectType(object source, Type type, bool ignoreNull = false)
        {
            var setting = GetSetting();
            if (ignoreNull)
            {
                setting.NullValueHandling = NullValueHandling.Ignore;
            }
            var json = JsonConvert.SerializeObject(source, setting);
            try
            {
                return JsonConvert.DeserializeObject(json, type, setting);
            }
            catch (Exception e)
            {
                if (type == typeof(string))
                    return Serialize(source, ignoreNull);
                throw e;
            }
        }

        /// <summary>
        /// Hàm thực hiện định cấu hình setting JsonConvert
        /// </summary>
        /// <returns></returns>
        public static JsonSerializerSettings GetSetting()
        {
            return new JsonSerializerSettings()
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                DateFormatString = "dd/MM/yyyy",
                NullValueHandling = NullValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }
    }
}
