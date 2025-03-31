using MauiDemo.ViewModels;

namespace MauiDemo.Views;

public partial class ChatStatsView : ContentPage
{
    private ChatStatsViewModel _viewModel;

    public ChatStatsView(ChatViewModel chatViewModel)
    {
        InitializeComponent();
        _viewModel = new ChatStatsViewModel(chatViewModel);
        BindingContext = _viewModel;
    }
}
