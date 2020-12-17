using API;
using API.Security;
using Azure.Core.Extensions;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using BusinessLogicLayer;
using BusinessLogicLayer.Business;
using BusinessLogicLayer.Business.Implementation;
using BusinessLogicLayer.Data.Central;
using BusinessLogicLayer.Data.Central.Implementation;
using BusinessLogicLayer.Security;
using BusinessModel.Security;
using Common.Security;
using JWT;
using JWT.Algorithms;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVCTemplate
{
	namespace API
	{
		public class Startup
		{
			public Startup(IConfiguration configuration)
			{
				Configuration = configuration;
				TestHelper.IsTesting = false;
			}

			public IConfiguration Configuration { get; }

			// This method gets called by the runtime. Use this method to add services to the container.
			public void ConfigureServices(IServiceCollection services)
			{
				services.AddControllers();
				services.AddHttpContextAccessor();
				services.AddSingleton<ICommonPrincipleAccessor, CommonPrincipleAccessor>();
				services.AddSingleton<IAlgorithmFactory, HMACSHAAlgorithmFactory>();
				services.AddSingleton<ICentralAuthRepository, CentralAuthRepository>();
				services.AddSingleton<ICentralAuthManager, CentralAuthManager>();
				services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;
				})
					.AddJwt(options =>
					{
						options.OnFailedTicket = OnFailedTicket;
						options.OnEmptyHeader = OnEmptyHeader;
						options.OnIncorrectScheme = OnIncorrectScheme;
					// secrets
					options.Keys = new[] { Constants.Secret };
						options.IdentityFactory = dict =>

						{
							return IdentityFactory.ClaimsToIdentityFactory(dict);
						};
					//*/
					//		options.IdentityFactory = dic => new ClaimsIdentity(
					//	dic.Select(p => new Claim(p.Key, p.Value)));
					/*
					
					*/
						options.TicketFactory = (identity, scheme) => new AuthenticationTicket(
							new CommonPrincipal((CommonIdentity)identity),
							new AuthenticationProperties(),
							scheme.Name);
					// force JwtDecoder to throw exception if JWT signature is invalid
					options.VerifySignature = true;
					});
				services.AddMvc();
				services.AddLocalization(options => options.ResourcesPath = "Resources");

				services.AddControllersWithViews()
					.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
					.AddDataAnnotationsLocalization();
				services.AddSwaggerGenNewtonsoftSupport();
				services.AddMvc().AddNewtonsoftJson(opts =>
				{
					opts.SerializerSettings.Converters.Add(new StringEnumConverter());
				});
				services.AddAzureClients(builder =>
				{
					builder.AddBlobServiceClient(Configuration["ConnectionStrings:StorageConnection:blob"], preferMsi: true);
					builder.AddQueueServiceClient(Configuration["ConnectionStrings:StorageConnection:queue"], preferMsi: true);
				});
				services.AddSwaggerGen(c =>
				{
					c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
					c.SwaggerGeneratorOptions.Servers.Add(new OpenApiServer
					{
						Url = "https://api.azurewebsites.net/", //replace with server id
						Description = "Dev Server"
					});
					//var filePath = Path.Combine(System.AppContext.BaseDirectory, "FoxHornPlayerCloud.xml");
					//c.IncludeXmlComments(filePath);
				});
				services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
			}

			private AuthenticateResult OnIncorrectScheme(ILogger arg1, string arg2, string arg3)
			{
				throw new NotImplementedException();
			}

			private AuthenticateResult OnEmptyHeader(ILogger arg1, string arg2)
			{
				throw new NotImplementedException();
			}

			private AuthenticateResult OnFailedTicket(ILogger arg1, Exception arg2)
			{
				throw new NotImplementedException();
			}

			// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
			public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
			{
				if (env.IsDevelopment())
				{
					app.UseDeveloperExceptionPage();
				}
				string baseDir = env.ContentRootPath;
				AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(baseDir, "App_Data"));
				var supportedCultures = new[] { "en", "es" };
				var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
					.AddSupportedCultures(supportedCultures)
					.AddSupportedUICultures(supportedCultures);

				app.UseRequestLocalization(localizationOptions);
				app.UseHttpsRedirection();

				app.UseAuthentication();
				app.UseRouting();

				app.UseAuthorization();

				app.UseEndpoints(endpoints =>
				{
					endpoints.MapControllers();
				});
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "API - V1");
				});
			}
		}
		internal static class StartupExtensions
		{
			public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
			{
				if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
				{
					return builder.AddBlobServiceClient(serviceUri);
				}
				else
				{
					return builder.AddBlobServiceClient(serviceUriOrConnectionString);
				}
			}
			public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions> AddQueueServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
			{
				if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
				{
					return builder.AddQueueServiceClient(serviceUri);
				}
				else
				{
					return builder.AddQueueServiceClient(serviceUriOrConnectionString);
				}
			}
		}
	}
}
