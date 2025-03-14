namespace MauiDemo.Models
{
    public enum MessageType
    {
        User,
        Bot
    }

    public class ChatMessage
    {
        public string Text { get; }
        public MessageType Type { get; }
        public DateTime Timestamp { get; }

        public ChatMessage(string text, MessageType type)
        {
            Text = text;
            Type = type;
            Timestamp = DateTime.Now;
        }
    }
}