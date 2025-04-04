using SQLite;
using System.Collections.Generic;

namespace MauiDemo.Models
{
    public class ChatConversation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime LastUpdatedAt { get; set; }
        
        [Ignore]
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        
        public ChatConversation()
        {
            CreatedAt = DateTime.Now;
            LastUpdatedAt = DateTime.Now;
            Title = $"Conversation {CreatedAt.ToString("yyyy-MM-dd HH:mm")}";
        }
        
        public ChatConversation(string title)
        {
            Title = title;
            CreatedAt = DateTime.Now;
            LastUpdatedAt = DateTime.Now;
        }
    }
}