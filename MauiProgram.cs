using Microsoft.Extensions.Logging;
using MauiDemo.Services;
using MauiDemo.Views;

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

        // Register views
        builder.Services.AddTransient<ChatBotView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
