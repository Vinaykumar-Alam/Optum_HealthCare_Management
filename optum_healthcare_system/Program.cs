using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using optum_healthcare_system.Middleware;
using optum_healthcare_system.Repository;
using optum_healthcare_system.Repository.Interfaces;
using optum_healthcare_system.Repository.Repository;

var builder = WebApplication.CreateBuilder(args);

//SWAGGER CONFIGURATION 
builder.Services.AddSwaggerGen(options=>{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Optum HealthCare System",
        Version = "v1"
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Optum HealthCare System 2.0",
        Version = "v2"
    });
});

//SERVICES
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//DATABASE CONNECTION
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=labresults.db");
});

//REPOSITORY INJECTION
builder.Services.AddScoped<ILabResultRepository, LabResultRepository>();
var app = builder.Build();

//DATA SEEDING
using (var scope = app.Services.CreateScope()){
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    DataSeeder.seedData(context);
}


// CONFIGURE THE HTTP REQUEST PIPELINE.
if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Optum HealthCare System");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "Optum HealthCare System 2.0");
        });
    }

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();
