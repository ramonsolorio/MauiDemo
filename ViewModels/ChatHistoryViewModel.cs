using MauiDemo.Models;
using MauiDemo.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiDemo.ViewModels
{
    public class ChatHistoryViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private ChatConversation? _selectedConversation;

        public ObservableCollection<ChatConversation> Conversations { get; }
        public ICommand LoadConversationCommand { get; }
        public ICommand DeleteConversationCommand { get; }
        public ICommand RefreshCommand { get; }

        public ChatConversation SelectedConversation
        {
            get => _selectedConversation!;
            set
            {
                if (_selectedConversation != value)
                {
                    _selectedConversation = value;
                    OnPropertyChanged();
                }
            }
        }

        public ChatHistoryViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Conversations = new ObservableCollection<ChatConversation>();

            LoadConversationCommand = new Command<ChatConversation>(async (conversation) =>
                await Shell.Current.GoToAsync($"//ChatBotView?conversationId={conversation.Id}"));

            DeleteConversationCommand = new Command<ChatConversation>(async (conversation) =>
                await DeleteConversation(conversation));

            RefreshCommand = new Command(async () => await LoadConversations());

        }

        public async Task LoadConversations()
        {
            Conversations.Clear();
            var conversations = await _databaseService.GetAllConversationsAsync();
            foreach (var conversation in conversations)
            {
                Conversations.Add(conversation);
            }
        }

        private async Task DeleteConversation(ChatConversation conversation)
        {
            if (conversation != null)
            {
                await _databaseService.DeleteConversationAsync(conversation);
                Conversations.Remove(conversation);
            }
        }

    }
}