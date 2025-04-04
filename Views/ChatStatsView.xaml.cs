using MauiDemo.ViewModels;

namespace MauiDemo.Views;

public partial class ChatStatsView : ContentPage
{
    private ChatStatsViewModel _viewModel;

    public ChatStatsView(ChatStatsViewModel chatStatsViewModel)
    {
        InitializeComponent();
        _viewModel = chatStatsViewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Refresh stats when the page appears
        await _viewModel.LoadHistoricalStatsAsync();
    }
}
