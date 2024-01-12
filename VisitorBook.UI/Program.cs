using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using VisitorBook.DAL.Data;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using VisitorBook.UI.Extensions;
using VisitorBook.UI.Middlewares;
using WebMarkupMin.AspNetCore7;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPermissionExt();

builder.Services.AddMappingExt();

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.AddServiceRepositoryExt();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection"));
});

builder.Services.AddIdentityExt();

builder.Services.AddCookieExt();

builder.Services.AddLocalizationExt();

builder.Services.AddConfigurationExt();

builder.Services.AddNotyf(config => { config.DurationInSeconds = 5; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

builder.Services.AddValidationExt();

builder.Services.AddWebMarkupMin()
    .AddHtmlMinification()
    .AddXmlMinification()
    .AddHttpCompression();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseRouting();

if (!app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}

app.UseAuthentication();

app.UseAuthorization();

app.UseNotyf();

app.UseWebMarkupMin();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();