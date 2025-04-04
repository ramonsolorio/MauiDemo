using MauiDemo.ViewModels;

namespace MauiDemo.Views;

public partial class ChatHistoryView : ContentPage
{
    private ChatHistoryViewModel _viewModel;

    public ChatHistoryView(ChatHistoryViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _viewModel.LoadConversations();
    }
}