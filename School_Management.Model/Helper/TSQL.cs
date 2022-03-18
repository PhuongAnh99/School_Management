using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Management.Model
{
    public static class TSQL
    {

        /// <summary>
        /// Hàm thực hiện biuld câu query lấy dữ liệu data phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelType"></param>
        /// <param name="columnOption"></param>
        /// <returns></returns>
        public static string BuildPagingData(PagingRequest request, Type modelType, List<string> columnOption = null)
        {
            var whereClause = BuildWhereClause(request, modelType);

            var columnString = "*";
            if (columnOption != null && columnOption.Count > 0)
            {
                columnString = string.Join(",", columnOption.Select(p => $"`{p}`"));
            }

            var model = (BaseModel)Activator.CreateInstance(modelType);
            var finalClause = $"select {columnString} from {model.GetTableName()} {whereClause} limit {request.PageSize} offset {(request.PageIndex - 1) * request.PageSize}";

            return finalClause;
        }

        /// <summary>
        /// Hàm thực hiện build câu query lấy tổng số bản ghi data phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public static string BuildTotalPagingData(PagingRequest request, Type modelType)
        {
            var whereClause = BuildWhereClause(request, modelType);

            var model = (BaseModel)Activator.CreateInstance(modelType);
            var finalClause = $"select count(*) as 'Total' from {model.GetTableName()} {whereClause}";

            return finalClause;
        }

        /// <summary>
        /// Hàm thực hiện build điều kiện WHERE từ filter UI đẩy lên
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public static string BuildWhereClause(PagingRequest request, Type modelType)
        {
            if (request.PageIndex <= 0 || request.PageSize <= 0)
                throw new Exception("Tham số pageindex và pagesize sai định dạng!");

            if (string.IsNullOrEmpty(request.Filter))
                return string.Empty;

            //Giải mã filter
            var jsonObj = request.Filter;
            var listFilterItem = TConvert.Deserialize<List<FilterItem>>(jsonObj);

            if (listFilterItem == null || listFilterItem.Count == 0)
                return string.Empty;

            PreventInjection(listFilterItem, modelType);

            var whereClause = string.Empty;
            foreach (var item in listFilterItem)
            {
                whereClause += MapPreFixFilterItem(item.Prefix);
                whereClause += MapConditionAndData(item.PropName, item.Condition, item.Value);
            }

            return $" where {whereClause}";
        }

        /// <summary>
        /// Hàm thực hiện map prefix của phần tử FilterItem
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private static string MapPreFixFilterItem(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return string.Empty;
            }

            switch (prefix.ToLower().Trim())
            {
                case "and":
                    return "and";
                case "or":
                    return "or";
                default:
                    throw new Exception("Prefix điều kiện filter sai định dạng!");
            }
        }

        /// <summary>
        /// Hàm thực hiện map điều kiện và data của phần tử FilterItem
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="condition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string MapConditionAndData(string propName, string condition, object value)
        {
            if (string.IsNullOrEmpty(condition))
            {
                throw new Exception("Condition bị trống");
            }

            var valueF = value;
            //Nếu value là string và khác rỗng
            //Ví dụ: value = "Phanh" => valueF = "'Phanh'"
            if (!string.IsNullOrEmpty(Convert.ToString(value)) && value.GetType().Name == typeof(string).Name)
            {
                valueF = $"'{valueF}'";       // 'valueF'
            }

            switch (condition.ToLower().Trim())
            {
                case "=":
                case "is":
                    return $" `{propName}` = {valueF} ";
                case "<>":
                case "is not":
                case "different":
                    return $" `{propName}` <> {valueF} ";
                case ">":
                case "bigger":
                    return $" `{propName}` > {valueF} ";
                case ">=":
                case "bigger than or equal":
                    return $" `{propName}` >= {valueF} ";
                case "<":
                case "smaller":
                    return $" `{propName}` < {valueF} ";
                case "<=":
                case "smaller than or equal":
                    return $" `{propName}` <= {valueF} ";
                case "=&all":
                    if (string.IsNullOrEmpty(Convert.ToString(value)))
                        return $" (`{propName}` is null or `{propName}` is not null) ";
                    else
                        return $" `{propName}` = {valueF} ";
                default:
                    throw new Exception("Condition điều kiện filter sai định dạng");
            }
        }

        /// <summary>
        /// Hàm thực hiện chống tấn công SQLInjection
        /// </summary>
        /// <param name="filterItems"></param>
        /// <param name="modelType"></param>
        private static void PreventInjection(List<FilterItem> filterItems, Type modelType)
        {
            if (filterItems.Count == 0 || filterItems == null)
            {
                return;
            }

            var properties = modelType.GetProperties();
            foreach (var item in filterItems)
            {
                if (properties?.Select(p => p.Name.ToLower()).Contains(item.PropName.ToLower()) == false)
                {
                    throw new Exception("Lỗi tấn công SQLInjection!");
                }
            }
            //Kiểm tra trong các property của model xem có chứa các filterItem.PropName không.
        }

        /// <summary>
        /// Hàm thực thi câu lệnh ExecuteScalar
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static object ExecuteScalarCommandText(string query, object param = null, DbTransaction transaction = null)
        {
            object result = null;
            var connection = transaction != null ? transaction.Connection : null;
            var command = connection.CreateCommand();

            if (command != null)
            {
                BuildCommand(ref command, query, param, CommandType.Text, transaction);
                result = command.ExecuteScalar();
            }
            else
            {
                using (connection = new SchoolContext().Database.GetDbConnection())
                {
                    connection.Open();
                    command = connection.CreateCommand();
                    BuildCommand(ref command, query, param, CommandType.Text, transaction);
                    result = command.ExecuteScalar();
                }
            }
            return result;
        }

        /// <summary>
        /// Hàm thực thi câu lệnh ExecuteScalarAsync
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<object> ExecuteScalarCommandTextAsync(string query, object param = null, DbTransaction transaction = null)
        {
            object result = null;
            var connection = transaction != null ? transaction.Connection : null;
            var command = connection.CreateCommand();

            if (command != null)
            {
                BuildCommand(ref command, query, param, CommandType.Text, transaction);
                result = await command.ExecuteScalarAsync();
            }
            else
            {
                using (connection = new SchoolContext().Database.GetDbConnection())
                {
                    connection.Open();
                    command = connection.CreateCommand();
                    BuildCommand(ref command, query, param, CommandType.Text, transaction);
                    result = await command.ExecuteScalarAsync();
                }
            }
            return result;
        }

        /// <summary>
        /// Hàm thực thi câu lệnh ExecuteReader
        /// </summary>
        /// <param name="modelTypes"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<object> ExecuteReaderCommandText(List<Type> modelTypes, string query, object param = null)
        {
            var result = new List<object>();
            using (var connection = new SchoolContext().Database.GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                BuildCommand(ref cmd, query, param, CommandType.Text);
                var reader = cmd.ExecuteReader();
                BuildDataReader(modelTypes, result, reader);
            }
            return result;
        }

        /// <summary>
        /// Hàm thực thi câu lệnh ExecuteReaderAsync
        /// </summary>
        /// <param name="modelTypes"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<List<object>> ExecuteReaderCommandTextAsync(List<Type> modelTypes, string query, object param = null)
        {
            var result = new List<object>();
            using (var connection = new SchoolContext().Database.GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                BuildCommand(ref cmd, query, param, CommandType.Text);
                var reader = await cmd.ExecuteReaderAsync();
                BuildDataReader(modelTypes, result, reader);
            }
            return result;
        }

        /// <summary>
        /// Hàm thực hiện build data object từ data reader
        /// </summary>
        /// <param name="modelTypes"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static void BuildDataReader(List<Type> modelTypes, List<object> result, DbDataReader reader)
        {
            var index = 0;
            do
            {
                var tableData = new List<object>();
                while (reader.Read())
                {
                    var activator = Activator.CreateInstance(modelTypes[index]);
                    var props = activator.GetType().GetProperties();
                    if (props == null || props.Count() == 0)
                        break;

                    foreach (var prop in props)
                    {
                        try
                        {
                            if (reader[prop.Name] == DBNull.Value)
                                activator.SetValue(prop.Name, null);
                            else
                                activator.SetValue(prop.Name, reader[prop.Name]);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    tableData.Add(activator);
                }
                result.Add(tableData);
                index++;
            } while (reader.NextResult());
        }

        /// <summary>
        /// Hàm thực hiện build mySql Command
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        private static void BuildCommand(ref DbCommand dbCommand, string commandText, object param, CommandType commandType, DbTransaction transaction = null)
        {
            dbCommand.CommandText = commandText;
            dbCommand.CommandType = commandType;
            dbCommand.Transaction = transaction;

            var mySQLParam = BuildMySqlParameters(new List<object>() { param });
            if (mySQLParam != null && mySQLParam.Count > 0)
            {
                dbCommand.Parameters.Add(mySQLParam);
            }
        }

        /// <summary>
        /// Hàm thực hiện build parameters cho MySQL Command
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static List<MySqlParameter> BuildMySqlParameters(List<object> data)
        {
            var result = new List<MySqlParameter>();
            if (data == null || data.Count == 0)
                return result;

            foreach (var obj in data)
            {
                var props = obj?.GetType()?.GetProperties();

                if (props == null || props.Count() == 0)
                    continue;

                foreach (var prop in props)
                {
                    if (prop.PropertyType.IsGenericType
                        || prop.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)
                        || prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        continue;

                    var param = new MySqlParameter();
                    param.ParameterName = $"v_{prop.PropertyType.Name}";
                    param.Value = prop.GetValue(obj);
                    switch (prop.PropertyType.Name.ToLower())
                    {
                        case "string":
                            param.MySqlDbType = MySqlDbType.VarChar;
                            if (string.IsNullOrEmpty(Convert.ToString(prop.GetValue(data))))
                                param.Value = DBNull.Value;
                            break;
                        case "decimal":
                            param.MySqlDbType = MySqlDbType.Decimal;
                            break;
                        case "datetime":
                            param.MySqlDbType = MySqlDbType.DateTime;
                            break;
                        case "int":
                            param.MySqlDbType = MySqlDbType.Int32;
                            break;
                        case "long":
                            param.MySqlDbType = MySqlDbType.Int64;
                            break;
                        case "bool":
                            param.MySqlDbType = MySqlDbType.Bit;
                            break;
                    }
                    result.Add(param);
                }
            }

            return result;
        }
    }
}
