using Microsoft.Extensions.Configuration;
namespace SemanticKernalTest.Credentials
{
    public static class Secrets
    {
        public static IConfiguration config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build();

        public static readonly string? openaiKey = config["OPENAI_KEY"];
        public static readonly string? openaiEndpoint = config["OPENAI_ENDPOINT"];
        public static readonly string? openaiChatModel = config["OPENAI_CHAT_MODEL"];
        public static readonly string? bingApiKey = config["BING_API_KEY"];
        public static readonly string? clientId = config["CLIENTID"];
        public static readonly string? tenantId = config["TENANTID"];
    }
}
