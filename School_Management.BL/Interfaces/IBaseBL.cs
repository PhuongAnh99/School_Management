using School_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Management.BL
{
    public interface IBaseBL<T> where T : BaseModel
    {
        /// <summary>
        /// Interface thực hiện thêm mới dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        Task<BaseResponse> Insert(T data);

        /// <summary>
        /// Interface thực hiện cập nhật dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        Task<BaseResponse> Update(T data);

        /// <summary>
        /// Interface thực hiện xóa dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        Task<BaseResponse> Delete(T data);

        /// <summary>
        /// Interface thực hiện lấy dữ liệu phân trang
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        Task<BaseResponse> GetPaging(PagingRequest request);
        Task<BaseResponse> GetAll();
        Task<BaseResponse> GetById(int id);
    }
}
