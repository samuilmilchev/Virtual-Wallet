using Microsoft.OpenApi.Models;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

      //  builder.Services.AddControllersWithViews() // Uncomment the lines below if you get the exception "JsonSerializationException: Self referencing loop detected for property..." 
      //                                         .AddNewtonsoftJson(options =>
      //                                         {
      //                                             options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
      //                                         })
      //;

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Gaming Forum API V1", Version = "v1" });
        });

        //builder.Services.AddDbContext<ApplicationContext>(options =>
        //{
        //    //    // The connection string can be found in the appsettings.json file. 
        //    //    // It's a good practice to keep the connection string in a separate file,
        //    //    //  because it's easier to change the connection string without recompiling the entire application.
        //    //    // Also, the connection string is a sensitive information and should not be exposed in the code.
        //    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        //    options.UseSqlServer(connectionString);

        //    //    // The following helps with debugging the trobled relationship between EF and SQL ¯\_(-_-)_/¯ 
        //    options.EnableSensitiveDataLogging();
        //});

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.UseDeveloperExceptionPage();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();
        app.UseSwagger();
        app.UseSwaggerUI(
               options =>
               {
                   options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gaming Forum API V1");
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