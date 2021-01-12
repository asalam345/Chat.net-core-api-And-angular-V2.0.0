using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;

using Models;
using Models.interfaces;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace chat_server.Service
{
	public class MySqlCommands
	{
        private readonly string _connectionString;
        DataTable dataTable;
        public MySqlCommands()
        {
            _connectionString = ConnectionStringManager.GetConnectionString();
        }
        public Result DefaultResult(string message)
        {
            Result result = new Result();
            result.Message = message;
            result.IsSuccess = true;
            result.Data = null;

            return result;
        }
        public async Task<long> getMaxRow(string columnName, string tableName)
        {
            try
            {
                string query = "SELECT MAX(" + columnName + ") FROM " + tableName + " WITH(NOLOCK)";
                DataTable dataTable = await GetData(query);
                long userId = dataTable.Rows.Count > 0 ? Convert.ToInt64(dataTable.Rows[0][0]) : 0;
                return userId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<bool> InsertOrUpdateOrDelete(string query)
		{
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, sql))
                    {
                        cmd.CommandType = CommandType.Text;
                        if (sql.State == ConnectionState.Closed)
                            await sql.OpenAsync();

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //exceptionHandel("Connection", "GetData", ex.ToString());
                return false;
            }
            finally
            {
            }
		}
		public async Task<DataTable> GetData(string query)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, sql))
                    {
                        cmd.CommandType = CommandType.Text;
                        if (sql.State == ConnectionState.Closed)
                            await sql.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
						{
                            dataTable = new DataTable();
                            dataTable.Load(reader);
						}
                    }
                }
                
               
                return dataTable;
            }
            catch (Exception ex)
            {
                //exceptionHandel("Connection", "GetData", ex.ToString());
                return null;
            }
            finally
            {
            }
        }
		
		//public bool exceptionHandel(string className, string methodName, string ex)
		//{
		//    bool result = false;

		//    try
		//    {
		//        result = InsertOrUpdate("INSERT INTO Exception(ClassName, MethodName, Exp, DateOfEx, TimeOfEx) " +
		//            "VALUES('" + className + "', '" + methodName + "', '" + ex + "', '" +
		//            DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToLongTimeString() + "')");
		//    }
		//    catch (Exception e)
		//    {
		//        exceptionHandel("Connection", "exceptionHandel", e.ToString());
		//    }
		//    return result;
		//}
	}
}
