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
    public class ChatViewModel : INotifyPropertyChanged
    {
        private readonly AzureOpenAIService _openAIService;
        private readonly List<OpenAI.Chat.ChatMessage> _messages;
        private string _userInput;
        private bool _isBotTyping;
        private ChatMessage _lastMessage;

        public ObservableCollection<ChatMessage> Messages { get; }
        public ICommand SendMessageCommand { get; }

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

        public ChatViewModel(AzureOpenAIService openAIService)
        {
            _openAIService = openAIService;
            Messages = new ObservableCollection<ChatMessage>();
            SendMessageCommand = new Command(async () => await SendMessage());

            _messages = new List<OpenAI.Chat.ChatMessage>
            {
                new SystemChatMessage("You are a helpful assistant.")
            };
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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
