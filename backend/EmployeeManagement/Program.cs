using EmployeeManagement.DatabaseConnection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// adding cors policy for trasnfering data that are hosted in another hosting platform.
var AllowLocalhost = "_AllowLocalhost";
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173",
                "http://localhost:5173/form")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


// it is going to configure sql server, using connection strings and connect with database.
builder.Services.AddDbContext<DatabaseContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
         )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// for cors policy.
app.UseCors(AllowLocalhost);


            // Configure the static files middleware to serve files from the "Images" folder

app.UseStaticFiles(new StaticFileOptions
{
            // Set the FileProvider to a PhysicalFileProvider pointing to the "Images" folder

    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath,"Images")),

    RequestPath = "/Images"         // Set the RequestPath to "/Images" so that requests to "/Images" will be served from the "Images" folder

});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


