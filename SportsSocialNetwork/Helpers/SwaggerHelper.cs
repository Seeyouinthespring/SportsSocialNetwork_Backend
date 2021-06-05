using System;
using System.IO;
using System.Reflection;
//using ICT4Kraam.SwaggerOperationFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SportsSocialNetwork.Attributes;

namespace SportsSocialNetwork.Helpers
{
    internal static class SwaggerHelper 
    {
        internal static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.ActionDescriptor.AttributeRouteInfo.Template}_{e.HttpMethod}");

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "beta",
                    Title = "SportSocialNetwork",
                    Description = "API for sports social network",
                    TermsOfService = new Uri("https://github.com/Seeyouinthespring/MEMAnalyzer_Backend/tree/main/MEMAnalyzer_Backend"),
                    Contact = new OpenApiContact
                    {
                        Name = "Nick Zhuravlyov",
                        Email = "colya.juravlyov2011@ya.ru",
                        Url = new Uri("https://vk.com/id118971987"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://vk.com/id118971987"),
                    }
                });

                #region Security definition

                var bearerScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                };
                c.AddSecurityDefinition("Bearer", bearerScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { bearerScheme, new string[] { } } });

            #endregion

            #region Operation filter
                c.OperationFilter<ServerErrorResponseOperationFilter>();
                c.SchemaFilter<ApplyIgnoreRelationshipsInNamespace>();
                #endregion

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();
                c.TagActionsBy(ApiDescriptionExtensions.GroupBySwaggerGroupAttribute);
            });
        }

        internal static void AddSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    } 
}
