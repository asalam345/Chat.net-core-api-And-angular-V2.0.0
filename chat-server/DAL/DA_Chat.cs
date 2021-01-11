using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DAL
{
	public class DA_Chat
	{
        private SignalRChatContext _ctx = null;
        public async Task<string[]> saveUserChat(tblMessage _model)
        {
            string[] message = new string[2];

            try
            {
                using (_ctx = new SignalRChatContext())
                {
                    DateTime dt = DateTime.Now;
                    tblMessage model = new tblMessage()
                    {
                        ChatId = _ctx.Message.DefaultIfEmpty().Max(x => x == null ? 0 : x.ChatId) + 1,
                        SenderId = _model.SenderId,
                        ReceiverId = _model.ReceiverId,
                        Message = _model.Message,
                        Date = dt,
                        Time = dt.ToShortTimeString()
                    };
                    _ctx.UserChat.Add(model);
                    await _ctx.SaveChangesAsync();
                    message[0] = model.ChatId.ToString();
                    message[1] = model.Time;
                }
            }
            catch (Exception ex)
            {
                message[0] = "Error:" + ex.ToString();
            }
            return message;
        }
        public async Task<string> DeleteOneSide(long chatId)
        {
            string message = string.Empty;
            try
            {
                using (_ctx = new SignalRChatContext())
                {
                    var uc = _ctx.UserChat.Where(w => w.ChatId == chatId).FirstOrDefault();
                    if (uc != null)
                    {
                        //DateTime dt = DateTime.Now;
                        //tblMessage model = new tblMessage()
                        //{
                        //    //ChatId = uc.ChatId,
                        //    uc.IsDelete = "o",
                        //    Date = uc.Date
                        //};
                        uc.IsDeleteFromReceiver = true;
                        _ctx.UserChat.Update(uc);
                        await _ctx.SaveChangesAsync();
                        message = "Saved";
                    }
                }

            }
            catch (Exception ex)
            {
                message = "Error:" + ex.InnerException;
            }

            return message;
        }
        public Task<bool> delete(long chatId)
        {
            return Task.Run(() =>
            {
                bool flag = false;
                tblMessage message = null;
                try
                {
                    using (_ctx = new SignalRChatContext())
                    {
                        message = _ctx.UserChat.Where(x => x.ChatId == chatId).FirstOrDefault();
                        _ctx.UserChat.Remove(message);
                        _ctx.SaveChanges();
                    }
                    flag = true;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    message = null;
                }

                return flag;
            });
        }
        public Task<List<tblMessage>> getUserChat(SenderAndReceiver model)
        {
            return Task.Run(() =>
            {
                List<tblMessage> userChat = null;
                try
                {
                    using (_ctx = new SignalRChatContext())
                    {
                        userChat = (from x in _ctx.UserChat
                                    where (x.SenderId == model.SenderId && x.ReceiverId == model.ReceiverId) || (x.ReceiverId == model.SenderId && x.SenderId == model.ReceiverId)
                                    select x).ToList();
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    userChat = null;
                }

                return userChat;
            });
        }
    }
}
