using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Options;
using SemanticKernalTest.Authentication;
using SemanticKernalTest.DTOs;
using SemanticKernalTest.Plugins;
using SemanticKernalTest.Plugins.NativePlugins;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RouteOptions>(options => options.SetParameterPolicy<RegexInlineRouteConstraint>("regex"));


builder.Services.AddCors();

var app = builder.Build();

app.UseSwagger(x => x.SerializeAsV2 = true);
app.UseSwaggerUI();

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
    options.AllowAnyMethod();
});


// Define minimal API endpoints
app.MapPost("/Chat", async (string userQuery) =>
{
    var plugin = new BasePlugin();
    // Call the BasicChat method on the instance
    var responseArray = new ResponseArray(
        chatResponse: "",
        serializedPlan: ""
    );

    responseArray = await plugin.AddPlugins(userQuery);

    var chatResponseList = new List<ChatResponse>
    {
        new ChatResponse
        {
            chatResponse = responseArray.ChatResponse,
            serializedPlan = responseArray.SerializedPlan
        }
    };


    // Return the chat response
    return Results.Ok(chatResponseList);

}).WithMetadata(new SwaggerOperationAttribute("Chat", "Chat"));


app.MapGet("/acquireaccesstoken", () => 
{
        var auth = new Auth();
        var token = auth.GetAccessToken();
        TokenManager.AccessToken = token;
        //Console.WriteLine(token);
        //return Results.Ok(token);
});

app.Run();

