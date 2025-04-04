using MauiDemo.Models;
using MauiDemo.Services;

namespace MauiDemo.ViewModels
{
    public class ChatStatsViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private int _userMessageCount;
        private int _botMessageCount;
        private int _totalMessageCount;
        private int _totalConversationsCount;

        public int UserMessageCount
        {
            get => _userMessageCount;
            set
            {
                if (_userMessageCount != value)
                {
                    _userMessageCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public int BotMessageCount
        {
            get => _botMessageCount;
            set
            {
                if (_botMessageCount != value)
                {
                    _botMessageCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TotalMessageCount
        {
            get => _totalMessageCount;
            set
            {
                if (_totalMessageCount != value)
                {
                    _totalMessageCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TotalConversationsCount
        {
            get => _totalConversationsCount;
            set
            {
                if (_totalConversationsCount != value)
                {
                    _totalConversationsCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public ChatStatsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task LoadHistoricalStatsAsync()
        {
            var conversations = await _databaseService.GetAllConversationsAsync();
            TotalConversationsCount = conversations.Count;

            int totalUserMessages = 0;
            int totalBotMessages = 0;

            foreach (var conversation in conversations)
            {
                var messages = await _databaseService.GetConversationMessagesAsync(conversation.Id);
                totalUserMessages += messages.Count(m => m.Type == MessageType.User);
                totalBotMessages += messages.Count(m => m.Type == MessageType.Bot);
            }

            // Update the stats with both current session and historical data
            UserMessageCount = totalUserMessages ;
            BotMessageCount = totalBotMessages;
            TotalMessageCount = totalUserMessages + totalBotMessages ;
        }

    }
}