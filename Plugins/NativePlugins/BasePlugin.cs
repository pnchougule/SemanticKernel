using Kernel = Microsoft.SemanticKernel.Kernel;
using Microsoft.SemanticKernel.Planning.Handlebars;
using Microsoft.SemanticKernel;
using SemanticKernalTest.DTOs;
using Microsoft.SemanticKernel.Plugins.Web.Bing;
using Microsoft.SemanticKernel.Plugins.Web;
using SemanticKernalTest.Authentication;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
namespace SemanticKernalTest.Plugins.NativePlugins
{

    public class BasePlugin
    {


        public async Task<ResponseArray> AddPlugins(string userQuery)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string? key = config["OPENAI_KEY"];
            string? endpoint = config["OPENAI_ENDPOINT"];
            string? model = config["OPENAI_CHAT_MODEL"];
            string? bingApiKey = config["BING_API_KEY"];
            //initialize the kernel
            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(model!, endpoint!, key!);
            var kernel = builder.Build();

            //import the default bing websearcher plugin into the kernel
            var bingConnector = new BingConnector(bingApiKey!);
            kernel.ImportPluginFromObject(new WebSearchEnginePlugin(bingConnector), "bing");
            var function = kernel.Plugins["bing"]["search"];

            //Import GraphPlugin into kernal
            var graphPlugin = kernel.ImportPluginFromType<GraphPlugin>();

            var planner = new HandlebarsPlanner(new HandlebarsPlannerOptions() { AllowLoops = true });
            Console.WriteLine("Sending Message To Chat Completions Model");

            var responseArray = new ResponseArray("", "");
            try
            {
                var plan = await planner.CreatePlanAsync(kernel, userQuery);

                var serializedPlan = plan.ToString();



                var result = await plan.InvokeAsync(kernel);

                var chatResponse = result.ToString();

                var stringSerializedPlan = serializedPlan.ToString();

                responseArray = new ResponseArray(chatResponse, stringSerializedPlan);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return responseArray;
        }


    }
}
