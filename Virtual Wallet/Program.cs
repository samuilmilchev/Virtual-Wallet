using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Text;
using Virtual_Wallet.Db;
using Virtual_Wallet.Helpers;
using Virtual_Wallet.Helpers.Contracts;
using Virtual_Wallet.Repository;
using Virtual_Wallet.Repository.Contracts;
using Virtual_Wallet.Services;
using Virtual_Wallet.Services.Contracts;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews() // Uncomment the lines below if you get the exception "JsonSerializationException: Self referencing loop detected for property..." 
                                               .AddNewtonsoftJson(options =>
                                               {
                                                   options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                                               })
      ;

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Virtual Wallet API V1", Version = "v1" });
        });

        builder.Services.AddDbContext<ApplicationContext>(options =>
        {
            //    // The connection string can be found in the appsettings.json file. 
            //    // It's a good practice to keep the connection string in a separate file,
            //    //  because it's easier to change the connection string without recompiling the entire application.
            //    // Also, the connection string is a sensitive information and should not be exposed in the code.
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);

            //    // The following helps with debugging the trobled relationship between EF and SQL ¯\_(-_-)_/¯ 
            options.EnableSensitiveDataLogging();
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.RequireHttpsMetadata = false; // Set to true if using HTTPS
               options.SaveToken = true;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AppSettings:Token"])),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };

               options.Events = new JwtBearerEvents
               {
                   OnMessageReceived = context =>
                   {
                       // Check for token in cookies
                       var token = context.Request.Cookies["jwt"];
                       if (token != null)
                       {
                           context.Token = token;
                       }
                       return Task.CompletedTask;
                   }
               };
           });

        builder.Services.AddAuthorization();

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;

        });

        // Add services to the container.

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ICardRepository, CardRepository>();
		builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

		builder.Services.AddScoped<IUsersService, UsersService>();
        builder.Services.AddScoped<ICardService, CardService>();
		builder.Services.AddScoped<ITransactionService, TransactionService>();

		builder.Services.AddScoped<IModelMapper, ModelMapper>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseDeveloperExceptionPage();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();
        app.UseSwagger();
        app.UseSwaggerUI(
               options =>
               {
                   options.SwaggerEndpoint("/swagger/v1/swagger.json", "Virtual Wallet API V1");
                   options.RoutePrefix = "swagger";
               }
           );

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            //endpoints.MapControllers();
            endpoints.MapSwagger();
        });

        app.Run();
    }
}