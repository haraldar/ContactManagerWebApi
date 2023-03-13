using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ContactManagerWebApi.Data;
using System.Reflection;

namespace ContactManagerWebApi;
/// <summary>
/// Handles the configuration and registration of the services to use with the API.
/// </summary>
public class Startup
{
    public IConfiguration Configuration { get; set; }


    public Startup(IConfiguration configuration)
        => Configuration = configuration;


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

        // Add the PostgreSQL services.
        services
            .AddEntityFrameworkNpgsql()
            .AddDbContext<ContactsDbContext>(options =>
                options.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"])
            );

        // Add our PostgreSQL Repositories (scoped to each request)
        services.AddScoped<IContactsRepository, ContactsRepository>();
        
        //Transient: Created each time they're needed
        services.AddTransient<ContactsDbSeeder>();

        // Add the endpoints.
        services.AddControllers();

        // Add Swagger support on a free endpoint.
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ContactManager WebAPI",
                Description = "Manages contact information in a PostgreSQL database.",
                Contact = new OpenApiContact { Name = "Harald Asmus" },
                License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://en.wikipedia.org/wiki/MIT_License") },
                
            });

              // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
        });

        // Add and configure the CORS policy.
        services.AddCors(o => o.AddPolicy("AllowAllPolicy", options =>
        {
            options.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
        }));

    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ContactsDbSeeder contactsDbSeeder)
    {
        // if (env.IsDevelopment())
        //     app.UseDeveloperExceptionPage();
        // else
        //     app.UseExceptionHandler("/Home/Error");
        // We don't need fancy Error Handling for a test WebAPI, therefore I opted for the given one.
        app.UseDeveloperExceptionPage();
        
        // Set CORS configuration.
        app.UseCors("AllowAllPolicy");

        // Enable middleware to serve generated Swagger as a JSON endpoint and swagger-ui assets (HTML, JS, CSS etc.)
        // on the endpoint /swagger.
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());

        // Populate the database with initial/ test data.
        contactsDbSeeder.SeedAsync(app.ApplicationServices).Wait();

    }

}
