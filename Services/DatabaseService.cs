using SQLite;
using MauiDemo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MauiDemo.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database; // Make field readonly
        private bool _initialized = false;
        private readonly SemaphoreSlim _initializationSemaphore = new SemaphoreSlim(1, 1);

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            //Initialize the database
            InitializeAsync().ConfigureAwait(false);
        }

        public async Task InitializeAsync()
        {
            // Only allow one thread to initialize the database
            await _initializationSemaphore.WaitAsync();

            try
            {
                if (!_initialized)
                {
                    await _database.CreateTableAsync<ChatConversation>();
                    await _database.CreateTableAsync<ChatMessage>();
                    _initialized = true;
                }
            }
            finally
            {
                _initializationSemaphore.Release();
            }
        }

        // Ensure database is initialized before any operation
        /*private async Task EnsureInitializedAsync()
        {
            if (!_initialized)
            {
                await InitializeAsync();
            }
        }*/

        // Conversation methods
        public async Task<List<ChatConversation>> GetAllConversationsAsync()
        {
            
            return await _database.Table<ChatConversation>().OrderByDescending(c => c.LastUpdatedAt).ToListAsync();
        }

        public async Task<ChatConversation> GetConversationAsync(int id)
        {
            
            var conversation = await _database.Table<ChatConversation>().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (conversation != null)
            {
                conversation.Messages = await GetConversationMessagesAsync(id);
            }
            return conversation;
        }

        public async Task<int> SaveConversationAsync(ChatConversation conversation)
        {
            
            if (conversation.Id != 0)
            {
                conversation.LastUpdatedAt = DateTime.Now;
                await _database.UpdateAsync(conversation);
                return conversation.Id;
            }
            else
            {
                // Insert the conversation
                await _database.InsertAsync(conversation);
                // Return the auto-incremented ID that was assigned
                return conversation.Id;
            }
        }

        public async Task<int> DeleteConversationAsync(ChatConversation conversation)
        {
            
            // First delete all messages in the conversation
            await _database.Table<ChatMessage>()
                .Where(m => m.ConversationId == conversation.Id)
                .DeleteAsync();

            // Then delete the conversation
            return await _database.DeleteAsync(conversation);
        }

        // Message methods
        public async Task<List<ChatMessage>> GetConversationMessagesAsync(int conversationId)
        {
            
            return await _database.Table<ChatMessage>()
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.Order)
                .ToListAsync();
        }

        public async Task<int> SaveMessageAsync(ChatMessage message, int conversationId)
        {
            
            message.ConversationId = conversationId;

            if (message.Id != 0)
            {
                return await _database.UpdateAsync(message);
            }
            else
            {
                return await _database.InsertAsync(message);
            }
        }

        public async Task SaveMessagesAsync(List<ChatMessage> messages, int conversationId)
        {
            
            for (int i = 0; i < messages.Count; i++)
            {
                messages[i].ConversationId = conversationId;
                messages[i].Order = i;                
                await SaveMessageAsync(messages[i], conversationId);
            }
        }
    }
}