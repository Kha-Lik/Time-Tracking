using APIGateway.Jwt;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllersWithViews();
builder.Services.AddOcelot()
    .AddConsul()
    .AddCacheManager(settings => settings.WithDictionaryHandle()); 
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddJwtAuthServices(builder.Configuration);
builder.Configuration.AddJsonFile(
$"ocelot.{builder.Environment.EnvironmentName}.json", true, true);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CurrentCorsPolicy",
        b =>
        {
            b.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.UseCors("CurrentCorsPolicy");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
await app.UseOcelot();
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});
app.Run();
