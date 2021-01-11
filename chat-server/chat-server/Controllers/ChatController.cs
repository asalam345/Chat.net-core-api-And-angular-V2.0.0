using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using chat_server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using DAL;
using Newtonsoft.Json;

namespace chat_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> hubContext;
        private DA_Chat _objAuthtication = null;
        public ChatController(IHubContext<ChatHub> hubContext)
        {
            this.hubContext = hubContext;
            this._objAuthtication = new DA_Chat();
        }
       
        [HttpPost("[action]")]
        public async Task SendMessage([FromBody] ChatMessage chat)
        {
            await this.hubContext.Clients.All.SendAsync("messageReceivedFromApi", chat);
            tblMessage message = new tblMessage();
            message.Message = chat.Text;
            message.ReceiverId = chat.ReceiverId;
            message.SenderId = chat.SenderId;
            await _objAuthtication.saveUserChat(message);
        }
        [HttpPost("[action]")]
        public async Task<object> getMessage([FromBody] SenderAndReceiver model)
        {
            object result = null; object resdata = null;

            try
            {
                if (model == null)
                {
                    return BadRequest();
                }
				
				resdata = await _objAuthtication.getUserChat(model);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            result = new
            {
                resdata
            };

            return result;
        }
        [HttpPut("[action]")]
        public async Task DeleteOneSide(long id)
        {
            await _objAuthtication.DeleteOneSide(id);
        }
        [HttpDelete("[action]")]
        public async Task Delete(long id)
        {
            await _objAuthtication.delete(id);
        }

    }
}
