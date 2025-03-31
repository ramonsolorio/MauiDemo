using System.ComponentModel;
using System.Runtime.CompilerServices;
using MauiDemo.Models;

namespace MauiDemo.ViewModels
{
    public class ChatStatsViewModel : INotifyPropertyChanged
    {
        private readonly ChatViewModel _chatViewModel;
        private int _userMessageCount;
        private int _botMessageCount;
        private int _totalMessageCount;

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

        public ChatStatsViewModel(ChatViewModel chatViewModel)
        {
            _chatViewModel = chatViewModel;
            UpdateStats();
            
            // Subscribe to changes in the messages to update statistics
            _chatViewModel.PropertyChanged += ChatViewModel_PropertyChanged;
        }

        private void ChatViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ChatViewModel.LastMessage))
            {
                UpdateStats();
            }
        }

        private void UpdateStats()
        {
            UserMessageCount = _chatViewModel.Messages.Count(m => m.Type == MessageType.User);
            BotMessageCount = _chatViewModel.Messages.Count(m => m.Type == MessageType.Bot);
            TotalMessageCount = _chatViewModel.Messages.Count;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}