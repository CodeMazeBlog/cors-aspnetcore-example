using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorsServerApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		private readonly string _policyName = "CorsPolicy";
		private readonly string _anotherPolicy = "AnotherCorsPolicy";

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(opt =>
			{
				opt.AddPolicy(name: _policyName, builder =>
				{
					builder.WithOrigins("https://localhost:5011")
						.AllowAnyHeader()
						.WithMethods("GET");
				});
				opt.AddPolicy(name: _anotherPolicy, builder =>
				{
					builder.WithOrigins("https://localhost:5021")
						.WithHeaders(HeaderNames.ContentType, HeaderNames.Accept)
						.WithMethods("GET")
						.WithExposedHeaders("X-Pagination");
				});
			});

			services.AddControllers();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();
			app.UseCors();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
