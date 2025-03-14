
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer(
    options =>
    {
        options.EmitStaticAudienceClaim = true; // Optional, useful for API access
    })
                .AddInMemoryApiScopes(InMemoryConfig.GetApiScopes())
                .AddInMemoryApiResources(InMemoryConfig.GetApiResources())
                .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
                .AddTestUsers(InMemoryConfig.GetUsers())
                .AddInMemoryClients(InMemoryConfig.GetClients())
                .AddDeveloperSigningCredential(); // Developer signing credentials (for dev mode)

builder.Services.AddRazorPages();  
builder.Services.AddControllersWithViews();     

//builder.Services.AddAuthentication("Cookies") // Set "Cookies" as the default scheme
//    .AddCookie("Cookies", options =>
//    {
//        options.LoginPath = "/Account/Login"; // Redirect to this path for unauthorized requests
//    });

builder.Services.AddAuthentication("Bearer")
   .AddJwtBearer("Bearer", opt =>
   {
       opt.RequireHttpsMetadata = false;
       opt.Authority = "https://localhost:5005";
       opt.Audience = "companyApi";
   });


builder.Services.AddAuthorization();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();  
app.MapControllers(); 

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
