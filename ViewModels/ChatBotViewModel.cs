using MauiDemo.Models;
using MauiDemo.Services;
using OpenAI.Chat;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ChatMessage = MauiDemo.Models.ChatMessage;

namespace MauiDemo.ViewModels
{
    public class ChatBotViewModel : BaseViewModel
    {
        private readonly AzureOpenAIService _openAIService;
        private readonly DatabaseService _databaseService;
        private readonly List<OpenAI.Chat.ChatMessage> _messages;
        private string _userInput;
        private bool _isBotTyping;
        private ChatMessage _lastMessage;
        private int _currentConversationId;

        public ObservableCollection<ChatMessage> Messages { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand SaveConversationCommand { get; }
        public ICommand NewConversationCommand { get; }

        public string UserInput
        {
            get => _userInput;
            set
            {
                if (_userInput != value)
                {
                    _userInput = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBotTyping
        {
            get => _isBotTyping;
            set
            {
                if (_isBotTyping != value)
                {
                    _isBotTyping = value;
                    OnPropertyChanged();
                }
            }
        }

        public ChatMessage LastMessage
        {
            get => _lastMessage;
            set
            {
                if (_lastMessage != value)
                {
                    _lastMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CurrentConversationId
        {
            get => _currentConversationId;
            set
            {
                if (_currentConversationId != value)
                {
                    _currentConversationId = value;
                    OnPropertyChanged();
                }
            }
        }

        public ChatBotViewModel(AzureOpenAIService openAIService, DatabaseService databaseService)
        {
            _openAIService = openAIService;
            _databaseService = databaseService;
            Messages = new ObservableCollection<ChatMessage>();
            SendMessageCommand = new Command(async () => await SendMessage());
            SaveConversationCommand = new Command(async () => await SaveConversation());
            NewConversationCommand = new Command(async () => await NewConversation());

            _messages = new List<OpenAI.Chat.ChatMessage>
            {
                new SystemChatMessage("You are a helpful assistant.")
            };

        }

        public async Task LoadConversationAsync(int conversationId)
        {
            var conversation = await _databaseService.GetConversationAsync(conversationId);
            if (conversation != null)
            {
                CurrentConversationId = conversation.Id;

                // Clear current messages
                Messages.Clear();
                _messages.Clear();
                _messages.Add(new SystemChatMessage("You are a helpful assistant."));

                // Add messages from database
                foreach (var message in conversation.Messages)
                {
                    Messages.Add(message);

                    // Add to OpenAI context
                    if (message.Type == MessageType.User)
                    {
                        _messages.Add(new UserChatMessage(message.Text));
                    }
                    else
                    {
                        _messages.Add(new AssistantChatMessage(message.Text));
                    }
                }

                if (Messages.Count > 0)
                {
                    LastMessage = Messages[Messages.Count - 1];
                }
            }
        }

        public async Task NewConversation()
        {
            // Clear messages
            Messages.Clear();

            // Reset OpenAI context
            _messages.Clear();
            _messages.Add(new SystemChatMessage("You are a helpful assistant."));

            _userInput = "";
            _isBotTyping = false;

            // Reset conversation ID and title
            CurrentConversationId = 0;

            // Reset LastMessage
            LastMessage = null;

            // Simulate async work
            await Task.CompletedTask;
        }

        public async Task SaveConversation()
        {
            if (Messages.Count == 0)
                return;

            var conversation = new ChatConversation(Messages.First().Text);

            if (CurrentConversationId != 0)
            {
                conversation.Id = CurrentConversationId;
            }

            // Save the conversation first to get its ID
            var conversationId = await _databaseService.SaveConversationAsync(conversation);
            CurrentConversationId = conversationId;

            // Convert ObservableCollection to List
            var messagesList = Messages.ToList();

            // Save all messages
            await _databaseService.SaveMessagesAsync(messagesList, conversationId);
        }

        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(UserInput))
                return;

            var userMessageText = UserInput.Trim();

            // Add user message to UI collection
            var userMessage = new ChatMessage(userMessageText, MessageType.User);
            Messages.Add(userMessage);
            LastMessage = userMessage;

            // Add message to OpenAI context
            _messages.Add(new UserChatMessage(userMessageText));

            // Clear input
            UserInput = string.Empty;

            // Show typing indicator
            IsBotTyping = true;

            // Get response
            string response = await _openAIService.GetCompletionAsync(_messages);

            // Hide typing indicator
            IsBotTyping = false;

            // Add bot response to UI collection
            var botMessage = new ChatMessage(response, MessageType.Bot);
            Messages.Add(botMessage);
            LastMessage = botMessage;

            // Add bot response to OpenAI context
            _messages.Add(new AssistantChatMessage(response));

            // Auto-save conversation after each message exchange
            await SaveConversation();
        }


    }
}
