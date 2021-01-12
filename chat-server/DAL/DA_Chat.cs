using Models;
using Models.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using chat_server.Service;
using System.Data;

namespace DAL
{
	public class DA_Chat : MySqlCommands, IGenericService<MessageVM>
    {
        public DA_Chat() 
        {
        }
        public async Task<Result> Get(MessageVM model)
		{
            Result result = DefaultResult("Success");

            try
            {
                IEnumerable<MessageVM> messages;

                if (model != null)
                {
                    string query = "SELECT * FROM tblMessage WITH(NOLOCK) WHERE SenderId = "
                        + model.SenderId + " AND ReceiverId = " + model.ReceiverId + " OR ReceiverId = "
                         + model.SenderId + " AND SenderId = " + model.ReceiverId;
                    DataTable dataTable = await GetData(query);
                    messages = Converter.ConvertDataTable<MessageVM>(dataTable);
                    result.Data = messages;
                }
            }
            catch (Exception ex)
            {
                result.Message = "UnSuccess";
                result.IsSuccess = false;
                result.Data = ex.Message;
            }

            return await Task.FromResult<Result>(result);
        }

		public async Task<Result> Entry(MessageVM model)
		{
            Result result = DefaultResult("Successfully!");
            try
			{
                long chatId = await getMaxRow("ChatId", "tblMessage") + 1;
                DateTime dt = DateTime.Now;
                string time = dt.ToShortTimeString();
                string query = @"INSERT INTO tblMessage(ChatId, SenderId,ReceiverId,Message,Date,Time) 
VALUES(" + chatId + "," + model.SenderId + "," + model.ReceiverId + ",'" + model.Message + "',GetDate(),'" 
+ time + "')";
                result.IsSuccess = await InsertOrUpdateOrDelete(query);
                //query = "SELECT LAST(ChatId) FROM tblMessage";
                string[] message = new string[2];
                message[0] = chatId.ToString();
                message[1] = time;
                result.Data = message;
            }
			catch (Exception ex)
			{
                result.IsSuccess = false;
                result.Message = "Error:" + ex.InnerException;
            }
            return result;
		}

		public async Task<Result> Update(MessageVM model)
		{
            Result result = DefaultResult("Successfully!");
            try
            {
                DateTime dt = DateTime.Now;
                string query = "UPDATE tblMessage SET IsDeleteFromReceiver = 1 WHERE ChatId=" + model.ChatId;
                result.IsSuccess = await InsertOrUpdateOrDelete(query);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Error:" + ex.InnerException;
            }

            return result;
        }

		public async Task<Result> Delete(long id)
		{
            Result result = DefaultResult("Successfully!");
            try
            {
                DateTime dt = DateTime.Now;
                string query = "Delete FROM tblMessage WHERE ChatId=" + id;
                result.IsSuccess = await InsertOrUpdateOrDelete(query);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Error:" + ex.InnerException;
            }

            return result;
        }
	}
}
