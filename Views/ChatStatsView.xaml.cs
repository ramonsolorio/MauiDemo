using MauiDemo.ViewModels;

namespace MauiDemo.Views;

public partial class ChatStatsView : ContentPage
{
    private ChatStatsViewModel _viewModel;

    public ChatStatsView(ChatBotViewModel chatBotViewModel)
    {
        InitializeComponent();
        _viewModel = new ChatStatsViewModel(chatBotViewModel);
        BindingContext = _viewModel;
    }
}