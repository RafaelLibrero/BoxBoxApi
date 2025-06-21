using Azure.Security.KeyVault.Secrets;
using BoxBoxApi.Data;
using BoxBoxApi.Helpers;
using BoxBoxApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using NSwag;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddTransient<RepositoryBoxBox>();
builder.Services.AddTransient<IChatRepository, ChatRepository>();

builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});

SecretClient secretClient =
builder.Services.BuildServiceProvider().GetService<SecretClient>();
KeyVaultSecret secret =
    await secretClient.GetSecretAsync("SqlAzure");
builder.Services.AddDbContext<BoxBoxContext>
    (options => options.UseSqlServer(secret.Value));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "Api BoxBox";
    document.Description = "Api BoxBox.  Proyecto Individual en Azure";
    document.AddSecurity("JWT", Enumerable.Empty<string>(),
        new NSwag.OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Copia y pega el Token en el campo 'Value:' así: Bearer {Token JWT}."
        }
    );
    document.OperationProcessors.Add(
    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});


HelperToken helper = new HelperToken(secretClient);
builder.Services.AddAuthentication(helper.GetAuthenticateSchema())
    .AddJwtBearer(helper.GetJwtBearerOptions());
builder.Services.AddTransient<HelperToken>(x => helper);
builder.Services.AddControllers();

var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        url: "/swagger/v1/swagger.json", 
        name: "Api BoxBox");
    options.RoutePrefix = "";
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
