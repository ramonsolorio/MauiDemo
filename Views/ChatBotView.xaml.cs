using MauiDemo.Models;
using MauiDemo.Services;
using MauiDemo.ViewModels;

namespace MauiDemo.Views;

public partial class ChatBotView : ContentPage
{
    private ChatViewModel _viewModel;

    public ChatBotView(AzureOpenAIService openAIService)
    {
        InitializeComponent();
        _viewModel = new ChatViewModel(openAIService);
        BindingContext = _viewModel;

        // Subscribe to property changes for LastMessage
        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ChatViewModel.LastMessage) && _viewModel.LastMessage != null)
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
