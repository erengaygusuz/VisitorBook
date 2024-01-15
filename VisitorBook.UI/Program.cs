using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using VisitorBook.DAL.Data;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using VisitorBook.UI.Extensions;
using VisitorBook.UI.Middlewares;
using WebMarkupMin.AspNetCore7;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement, UnicodeRanges.LatinExtendedA }));

builder.Services.AddPermissionExt();

builder.Services.AddMappingExt();

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

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

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

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "StaticFiles")),
    RequestPath = "/static-files"
});

app.UseNotyf();

app.UseWebMarkupMin();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();