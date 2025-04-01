using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Reflection;
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

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MauiDemo.appsettings.json");
        if (stream == null)
        {
            throw new InvalidOperationException("Failed to load the embedded resource 'MauiDemo.appsettings.json'.");
        }
        var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
        var endpoint = config["AzureOpenAI:Endpoint"] ?? throw new InvalidOperationException("AzureOpenAI:Endpoint is not configured.");
        var apiKey = config["AzureOpenAI:ApiKey"] ?? throw new InvalidOperationException("AzureOpenAI:ApiKey is not configured.");
        var deploymentName = config["AzureOpenAI:DeploymentName"] ?? throw new InvalidOperationException("AzureOpenAI:DeploymentName is not configured.");
        
        builder.Services.AddSingleton(new AzureOpenAIService(endpoint, apiKey, deploymentName));

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
