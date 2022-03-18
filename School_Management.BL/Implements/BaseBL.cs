using Microsoft.EntityFrameworkCore;
using School_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static School_Management.Model.Enumerations;

namespace School_Management.BL
{
    public class BaseBL<T> : IBaseBL<T> where T : BaseModel
    {
        protected readonly SchoolContext _dbcontext;
        public BaseBL(SchoolContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        /// <summary>
        /// Hàm thực hiện xóa dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public async Task<BaseResponse> Delete(T data)
        {
            var res = new BaseResponse();

            var table = _dbcontext.Set<T>();
            table.Remove(data);
            var effect = await _dbcontext.SaveChangesAsync();
            if (effect < 1)
            {
                return res.OnError(Code.ErrorCRUD, userMessage: "Xóa thất bại", devMessage: "Lỗi effect save change");
            }

            res.Onsuccess();
            return res;
        }

        /// <summary>
        /// Hàm thực hiện lấy dữ liệu phân trang
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetPaging(PagingRequest request)
        {
            var res = new BaseResponse();
            Type modelType = typeof(T);

            var sqlDataQuery = TSQL.BuildPagingData(request, modelType);
            var sqlTotalQuery = TSQL.BuildTotalPagingData(request, modelType);

            var pageData = TSQL.ExecuteReaderCommandTextAsync(new List<Type> { modelType }, sqlDataQuery);
            var totalData = TSQL.ExecuteScalarCommandTextAsync(sqlTotalQuery);

            await Task.WhenAll(pageData, totalData);
            var pagingRes = new PagingResponse()
            {
                PageData = TConvert.Deserialize<List<T>>(TConvert.Serialize(pageData.Result.FirstOrDefault())),
                Total = Convert.ToInt32(totalData.Result)
            };
            return res.Onsuccess(pagingRes);
        }

        /// <summary>
        /// Hàm thực hiên lấy tất cả dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<BaseResponse> GetAll()
        {
            var res = new BaseResponse();
            var table = _dbcontext.Set<T>();
            var tableList = await table.ToListAsync();
            return res.Onsuccess(tableList);
        }

        /// <summary>
        /// Hàm thực hiện lấy dữ liệu theo Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetById(int id)
        {
            var res = new BaseResponse();
            var entity = await _dbcontext.Set<T>().FindAsync(id);
            return res.Onsuccess(entity);
        }

        /// <summary>
        /// Hàm thực hiện thêm mới dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public async Task<BaseResponse> Insert(T data)
        {
            var res = new BaseResponse();

            var table = _dbcontext.Set<T>();
            table.Add(data);
            var effect = await _dbcontext.SaveChangesAsync();
            if (effect <= 0)
            {
                return res.OnError(Code.ErrorCRUD, userMessage: "Thêm thất bại", devMessage: "Lỗi effect save change");
            }
            res.Onsuccess();
            return res;
        }

        /// <summary>
        /// Hàm thực hiện cập nhật dữ liệu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public async Task<BaseResponse> Update(T data)
        {
            var res = new BaseResponse();

            var table = _dbcontext.Set<T>();
            table.Update(data);
            var effect = await _dbcontext.SaveChangesAsync();
            if (effect < 1)
            {
                return res.OnError(Code.ErrorCRUD, userMessage: "Cập nhật thất bại", devMessage: "Lỗi effect save change");
            }

            res.Onsuccess();
            return res;
        }
    }
}
