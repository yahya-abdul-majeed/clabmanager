using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "node_modules")),
//    RequestPath = "/node_modules"
//});

//app.UseFileServer(new FileServerOptions()
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")),
//    RequestPath = new PathString("/node_modules"),
//    EnableDirectoryBrowsing = true
//});

app.UseStaticFiles();

app.UseRouting();


app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area=Admin}/{controller=Issues}/{action=Index}/{id?}"
);


app.Run();
