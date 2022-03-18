using Microsoft.AspNetCore.Mvc;
using School_Management.BL;
using School_Management.Model;
using System;
using System.Threading.Tasks;
using static School_Management.Model.Enumerations;

namespace School_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase where T : BaseModel
    {
        protected IBaseBL<T> BL { get; set; }

        /// <summary>
        /// API thực hiện lấy toàn bộ dữ liệu
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<BaseResponse> GetAll()
        {
            var res = new BaseResponse();
            try
            {
                res = await BL.GetAll();
            }
            catch (Exception ex)
            {
                res.OnError(Code.Exception, devMessage: ex.Message, exception: ex);
            }
            return res;
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse> GetById([FromBody] int id)
        {
            var res = new BaseResponse();
            try
            {
                res = await BL.GetById(id);
            }
            catch (Exception ex)
            {
                res.OnError(Code.Exception, devMessage: ex.Message, exception: ex);
            }
            return res;
        }

        /// <summary>
        /// API thực hiện thêm mới dữ liệu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<BaseResponse> Insert([FromBody] T data)
        {
            var res = new BaseResponse();
            try
            {
                res = await BL.Insert(data);
            }
            catch (Exception ex)
            {
                res.OnError(Code.Exception, devMessage: ex.Message, exception: ex);
            }
            return res;
        }

        /// <summary>
        /// API thực hiện cập nhật dữ liệu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut()]
        public async Task<BaseResponse> Update([FromBody] T data)
        {
            var res = new BaseResponse();
            try
            {
                res = await BL.Update(data);
            }
            catch (Exception ex)
            {
                res.OnError(Code.Exception, devMessage: ex.Message, exception: ex);
            }
            return res;
        }

        /// <summary>
        /// API thực hiện xóa dữ liệu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete()]
        public async Task<BaseResponse> Delete([FromBody] T data)
        {
            var res = new BaseResponse();
            try
            {
                res = await BL.Delete(data);
            }
            catch (Exception ex)
            {
                res.OnError(Code.Exception, devMessage: ex.Message, exception: ex);
            }
            return res;
        }

        /// <summary>
        /// API thực hiện lấy dữ liệu phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("paging")]
        public async Task<BaseResponse> GetPaging([FromBody] PagingRequest request)
        {
            var res = new BaseResponse();

            try
            {
                res = await BL.GetPaging(request);
            }
            catch (Exception ex)
            {
                res.OnError(Code.Exception, devMessage: ex.Message, exception: ex);
            }

            return res;
        }
    }
}
