using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DigiBlog.Api.Repositories;
using DigiBlog.Api.Services;
using DigiBlog.Api.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DigiBlog.Api
{
	public class Startup
	{
		private readonly ILogger logger;

		public Startup(IConfiguration configuration, ILogger<Startup> logger)
		{
			Configuration = configuration;
			this.logger = logger;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
				loggingBuilder.AddConsole();
				loggingBuilder.AddDebug();
			});

			// cache
			services.AddMemoryCache();
			services.AddResponseCaching();

			var authSettings = Configuration.GetSection("AuthSettings");
			services.Configure<AuthSettings>(authSettings);

			var cacheSettings = Configuration.GetSection("CacheSettings");
			services.Configure<CacheSettings>(cacheSettings);

			var secret = authSettings.GetValue<string>("Secret");
			var key = Encoding.UTF8.GetBytes(secret);
			var symmetricKey = new SymmetricSecurityKey(key);

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.Events = new JwtBearerEvents
					{
						OnTokenValidated = context =>
						{
							var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
							var userId = context.Principal.Identity.Name;
							var user = userService.ReadSingleAsync(userId);
							if (user == null)
							{
								// return unauthorized if user no longer exists
								context.Fail("Unauthorized");
							}
							return Task.CompletedTask;
						}
					};
					options.RequireHttpsMetadata = false;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = authSettings.GetValue<string>("Issuer"),
						ValidAudience = authSettings.GetValue<string>("Audience"),
						IssuerSigningKey = symmetricKey
					};
				});

			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			// .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
				options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
			});

			services.AddScoped<IRepository<User>, UserRepository>();
			services.AddScoped<IRepository<Role>, RoleRepository>();
			services.AddScoped<IRepository<Category>, CategoryRepository>();
			services.AddScoped<IRepository<Article>, ArticleRepository>();
			services.AddScoped<IRepository<Comment>, CommentRepository>();

			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IRoleService, RoleService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IArticleService, ArticleService>();
			services.AddScoped<ICommentService, CommentService>();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}
			app.UseResponseCaching();
			app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseMvc();
		}
	}
}
