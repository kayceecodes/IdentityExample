using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("ApplicationDbContext"));

builder.Services.AddIdentityApiEndpoints<User>(
    opt => 
    { 
        opt.Password.RequiredLength = 8; 
        opt.User.RequireUniqueEmail = true; 
        opt.Password.RequireNonAlphanumeric = false;
        opt.SignIn.RequireConfirmedEmail = true; 
    })
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {   // Opens at http://localhost:(whatever port numer)/index.html
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
        options.RoutePrefix = string.Empty; // Set the Swagger UI at the root URL
    });
}


app.MapIdentityApi<User>();
app.ConfigurationAuthEndpoints();

app.MapGet("/", () => Results.Ok("Hello World!"));

app.Run();
