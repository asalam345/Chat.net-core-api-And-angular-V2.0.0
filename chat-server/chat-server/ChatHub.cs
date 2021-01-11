using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.SignalR;
using Models;
using System;
namespace chat_server
{
    public class ChatHub : Hub<IChatHub>
    {
		private DA_Chat _objAuthtication = null;
		public async Task BroadcastAsync(ChatMessage chat)
        {
			try
			{
                this._objAuthtication = new DA_Chat();
                tblMessage message = new tblMessage();
                message.Message = chat.Text;
                message.ReceiverId = chat.ReceiverId;
                message.SenderId = chat.SenderId;
                string[] result = await _objAuthtication.saveUserChat(message);
                if (result.Length > 1)
                {
                    chat.ChatId = Convert.ToInt64(result[0].Trim());
                    chat.Time = result[1];
                    await Clients.All.MessageReceivedFromHub(chat);
                }
            }
			catch (Exception)
			{

			}
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.NewUserConnected("a new user connectd");
        }
	}

    public interface IChatHub
    {
        Task MessageReceivedFromHub(ChatMessage message);

        Task NewUserConnected(string message);
    }
}
