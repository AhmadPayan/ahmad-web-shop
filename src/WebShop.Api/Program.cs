using WebShop.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration); // Extension method to add services from WebShop.Application

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebShop.Api v1"));
}

app.UseHttpsRedirection();
app.UseRouting()
.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();