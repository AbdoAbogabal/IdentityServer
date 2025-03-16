
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.EntityFramework.DbContexts;
using identityServerWeb.Entities;
using Microsoft.AspNetCore.Identity;
using identityServerWeb.DBContext.ApplicationDbContext;
using identityServerWeb.DBContext.ConfigurationDbContext;
using identityServerWeb.DBContext.PersistedGrantDbContext;
using IdentityServerHost;

var builder = WebApplication.CreateBuilder(args);


var connectionString = "server=.; database=DuendeIdentityServerDB; Integrated Security=true;TrustServerCertificate=True";

builder.Services.AddDbContext<ConfigurationAppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<PersistedGrantAppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentityServer()
    .AddConfigurationStore<ConfigurationDbContext>(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(connectionString);
    })
    .AddOperationalStore<PersistedGrantDbContext>(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(connectionString);
    }).AddTestUsers(TestUsers.Users);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


#region  Register 
//builder.Services.AddAuthentication("Cookies") // Set "Cookies" as the default scheme
//    .AddCookie("Cookies", options =>
//    {
//        options.LoginPath = "/Account/Login"; // Redirect to this path for unauthorized requests
//    });

//builder.Services.AddAuthentication("Bearer")
//   .AddJwtBearer("Bearer", opt =>
//   {
//       opt.RequireHttpsMetadata = false;
//       opt.Authority = "https://localhost:5005";
//       opt.Audience = "companyApi";
//   });
#endregion


builder.Services.AddAuthorization();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var configContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
    configContext.Database.Migrate();

    var persistedGrantContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
    persistedGrantContext.Database.Migrate();

    var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    appContext.Database.Migrate();

    // Seed configuration data
    if (!configContext.Clients.Any())
    {
        foreach (var client in InMemoryConfig.GetClients())
        {
            configContext.Clients.Add(client.ToEntity());
        }
        configContext.SaveChanges();
    }

    if (!configContext.IdentityResources.Any())
    {
        foreach (var resource in InMemoryConfig.GetIdentityResources())
        {
            configContext.IdentityResources.Add(resource.ToEntity());
        }
        configContext.SaveChanges();
    }

    if (!configContext.ApiScopes.Any())
    {
        foreach (var scp in InMemoryConfig.GetApiScopes())
        {
            configContext.ApiScopes.Add(scp.ToEntity());
        }
        configContext.SaveChanges();
    }
}



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
