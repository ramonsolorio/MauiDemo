using MauiDemo.Models;
using MauiDemo.Services;
using MauiDemo.ViewModels;

namespace MauiDemo.Views;

[QueryProperty(nameof(ConversationId), "conversationId")]
public partial class ChatBotView : ContentPage
{
    private ChatBotViewModel _viewModel;
    private string _conversationId;

    public string ConversationId
    {
        get => _conversationId;
        set
        {
            _conversationId = value;
            if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int id) && id > 0)
            {
                LoadConversation(id);
            }
        }
    }

    public ChatBotView(AzureOpenAIService openAIService, DatabaseService databaseService, ChatBotViewModel viewModel = null)
    {
        InitializeComponent();
        _viewModel = viewModel ?? new ChatBotViewModel(openAIService, databaseService);
        BindingContext = _viewModel;

        // Subscribe to property changes for LastMessage
        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private async void LoadConversation(int id)
    {
        await _viewModel.LoadConversationAsync(id);
        ScrollToLastMessage();
    }

    private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ChatBotViewModel.LastMessage) && _viewModel.LastMessage != null)
        {
            // Scroll to the last message
            ScrollToLastMessage();
        }
    }

    private void ScrollToLastMessage()
    {
        if (_viewModel.Messages.Count > 0)
        {
            MessageList.ScrollTo(_viewModel.Messages.Count - 1, position: ScrollToPosition.End, animate: true);
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Ensure we scroll to the last message when the page appears
        ScrollToLastMessage();
    }
}