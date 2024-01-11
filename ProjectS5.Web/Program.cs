using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectS5.Core.Auth;
using ProjectS5.Core.ServiceExtensions;
using ProjectS5.Core.Users.Models;
using ProjectS5.Data;
using ProjectS5.Data.Configurations;
using ProjectS5.Web.Components;
using ApplicationUser = ProjectS5.Data.ApplicationUser;
using Blazored.LocalStorage;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddRazorComponents()
.AddInteractiveServerComponents();

var connectionStrings = configuration.GetSection(ConnectionStringsConfig.ConnectionStrings).Get<ConnectionStringsConfig>();
services.AddDbContextPool<ProjectS5DbContext>(o => o.UseSqlServer(connectionStrings.ProjectS5Database));
services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ProjectS5DbContext>();
services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = false;
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

services.RegisterCoreServices();
services.AddEmail(configuration);
services.AddMemoryCache();

services.AddAutoMapper(typeof(RegisterUserModel).Assembly);
services.AddHttpContextAccessor();
services.AddOptions();
services.AddAuthorizationCore();
services.AddScoped<CustomStateProvider>();
services.AddBlazoredLocalStorage();
services.AddRadzenComponents();
services.AddCascadingAuthenticationState();
services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());
services.AddRazorComponents()
       .AddInteractiveServerComponents();
services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });


var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
var context = scope.ServiceProvider.GetRequiredService<ProjectS5DbContext>();
await context.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
