using AuraIDE.Services;
using AuraIDE.Models;
using Aura.IDE.RustCompiler.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IRustService, RustService>();
builder.Services.AddTransient<IHttpService, HttpService>();
builder.Services.AddScoped<IBlockchainService, BlockchainService>();
builder.Services.AddSession();

var auraSection = builder.Configuration.GetSection("AuraConfig");
var apiSection = builder.Configuration.GetSection("APIConfig");
var blockChainAPISection = builder.Configuration.GetSection("BlockchainAPIConfig");
builder.Services.Configure<AuraConfig>(auraSection);
builder.Services.Configure<APIConfig>(apiSection);
builder.Services.Configure<BlockchainAPIConfig>(blockChainAPISection);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
