using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static School_Management.Model.Enumerations;

namespace School_Management.Model
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public Code Code { get; set; }
        public object Data { get; set; }
        public string UserMessage { get; set; }
        public string DevMessage { get; set; }

        public BaseResponse(bool success = true, Code code = Code.Success, object data = null, string userMessage = "Thành Công!", string devMessage = "Done!")
        {
            Success = success;
            Code = code;
            Data = data;
            UserMessage = userMessage;
            DevMessage = devMessage;
        }

        /// <summary>
        /// Hàm bắn thành công Response
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userMessage"></param>
        /// <param name="devMessage"></param>
        /// <returns></returns>
        public BaseResponse Onsuccess(object data = null, string userMessage = "Thành công!", string devMessage = "Done!")
        {
            this.Success = true;
            this.Code = Code.Success;
            this.Data = data;
            this.UserMessage = userMessage;
            this.DevMessage = devMessage;
            return this;
        }

        /// <summary>
        /// Hàm bắn lỗi Response
        /// </summary>
        /// <param name="code"></param>
        /// <param name="userMessage"></param>
        /// <param name="devMessage"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public BaseResponse OnError(Code code, string userMessage = "Hệ thống xảy ra lỗi!", string devMessage = "Exception!", Exception exception = null)
        {
            this.Success = false;
            this.Code = code;
            this.Data = null;
            this.UserMessage = userMessage;
            this.DevMessage = devMessage;
            return this;
        }
    }
}
