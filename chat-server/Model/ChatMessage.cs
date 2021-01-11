using System;

namespace Models
{
    public class ChatMessage
    {
        public string Text { get; set; }
        public string ConnectionId { get; set; }
        public DateTime DateTime { get; set; }
        public string Time { get; set; }
        public long ReceiverId { get; set; }
        public long SenderId { get; set; }
        public long ChatId { get; set; }
    }
}
