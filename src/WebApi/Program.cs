using WebApi.Config;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var identityIntelligenceConfiguration = new IdentityIntelligenceConfiguration();

builder.Configuration.Bind("IdentityIntelligence", identityIntelligenceConfiguration);
identityIntelligenceConfiguration.Validate();
builder.RegisterServices(identityIntelligenceConfiguration);

var app = builder.Build().Configure();

app.Run();
