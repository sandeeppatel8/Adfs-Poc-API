using Adfs_Poc_API.Entity;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Adfs_Poc_API
{
    public static class SwaggerConfig
    {
        public static void SwaggerConfigurations(IServiceCollection Services, IConfiguration Configuration, SwaggerAzureAdSettings bindAzureAdSettings)
        {
            Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "ADFS Demo", Version = "v1" });
                option.DocumentFilter<HideSwaggerFilter>();
                option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description = "Oauth 2.0 Authorization code flow",
                    Name = "oauth 2.0",
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(bindAzureAdSettings.AuthorizationUrl),
                            TokenUrl = new Uri(bindAzureAdSettings.TokenUrl),
                            Scopes = new Dictionary<string, string>
                {
                    {bindAzureAdSettings.Scopes,"Access Api User" }
            }
                        }
                    }
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"
                                },
                                Scheme = "oauth2",
                                Name = "oauth2",
                            },
                            new List<string>(){ bindAzureAdSettings.Scopes}
                        }
                });
            });

        }

        public static void SwaggerUIConfiguration(IApplicationBuilder app, IConfiguration Configuration, IWebHostEnvironment env)
        {
            if (app != null)
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PCT Web API v1");
                    options.DefaultModelsExpandDepth(-1);
                    options.OAuthClientId(Configuration["SwaggerAzureAd:ClientId"]);
                    options.OAuthUsePkce();
                });

            }
        }
    }

    internal class HideSwaggerFilter : IDocumentFilter
    {
        private static readonly string[] _ignoredPaths = {
            "/configuration",
            "/outputcache/{region}"
        };


        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var ignorePath in _ignoredPaths)
            {
                swaggerDoc.Paths.Remove(ignorePath);
            }
        }
    }
}
