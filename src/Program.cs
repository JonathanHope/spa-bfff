using ProxyKit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddResponseCompression();
builder.Services.AddProxy();

var app = builder.Build();

app.UseResponseCompression();
app.UseStaticFiles();
app.UseRouting();
// TODO: Setup auth here.
app.MapRazorPages();

app.Map("/api", api => {
  api.RunProxy(async context => {
    var forwardContext = context.ForwardTo("https://postman-echo.com");
    // TODO: Add auth header here.
    return await forwardContext.Send();
  });
});

var isDev = app.Services.GetRequiredService<IWebHostEnvironment>().IsDevelopment();
if (isDev)
  app.UseSpa(spa => spa.UseProxyToSpaDevelopmentServer("http://localhost:3000"));
else
  app.UseSpa(spa => {});
  
app.Run();
