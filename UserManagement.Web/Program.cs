using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Westwind.AspNetCore.Markdown;
using UserManagement.Data; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddSingleton<IDataContext, DataContext>()
    .AddDomainServices()
    .AddMarkdown()
    .AddControllersWithViews();

var app = builder.Build();

app.UseMarkdown();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
