using Microsoft.Extensions.Logging;
using MauiDemo.Services;
using MauiDemo.Views;
using MauiDemo.ViewModels;

namespace MauiDemo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register Azure OpenAI service
        builder.Services.AddSingleton(new AzureOpenAIService(
            endpoint: "YOUR_AZURE_OPENAI_ENDPOINT",
            key: "YOUR_AZURE_OPENAI_KEY",
            deploymentName: "YOUR_DEPLOYMENT_NAME"
        ));

        // Register ChatViewModel as singleton to share between views
        builder.Services.AddSingleton<ChatViewModel>();

        // Register views
        builder.Services.AddTransient<ChatBotView>();
        builder.Services.AddTransient<ChatStatsView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
