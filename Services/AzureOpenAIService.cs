using Azure;
using Azure.AI.OpenAI;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace MauiDemo.Services;

public class AzureOpenAIService
{
    private readonly string _endpoint;
    private readonly string _key;
    private readonly string _deploymentName;
    private readonly AzureOpenAIClient _client;
    private readonly ChatClient _chatClient;

    public AzureOpenAIService(string endpoint, string key, string deploymentName)
    {
        _endpoint = endpoint;
        _key = key;
        _deploymentName = deploymentName;

        _client = new(
            new Uri(_endpoint),
            new ApiKeyCredential(_key));

        _chatClient = _client.GetChatClient(_deploymentName);
    }

    public async Task<string> GetCompletionAsync(List<ChatMessage> messages)
    {
        try
        {
            ChatCompletion completion = await _chatClient.CompleteChatAsync(messages);
            return completion.Content[0].Text;
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}
