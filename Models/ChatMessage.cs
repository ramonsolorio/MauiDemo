namespace MauiDemo.Models
{
    public enum MessageType
    {
        User,
        Bot
    }

    public class ChatMessage
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement, SQLite.Column("id")]
        public int Id { get; set; }
        
        [SQLite.Column("conversation_id")]
        public int ConversationId { get; set; }
        
        [SQLite.Column("message_text")]
        public string Text { get; set; }
        
        [SQLite.Column("message_type")]
        public MessageType Type { get; set; }
        
        [SQLite.Column("timestamp")]
        public DateTime Timestamp { get; set; }
        
        [SQLite.Column("order_field")]
        public int Order { get; set; }

        public ChatMessage(string text, MessageType type)
        {
            Text = text;
            Type = type;
            Timestamp = DateTime.Now;
        }
        
        // Parameterless constructor for SQLite
        public ChatMessage() 
        {
            Timestamp = DateTime.Now;
        }
    }
}