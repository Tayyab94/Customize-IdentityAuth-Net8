using Identity_Net8.HelperFunctions;
using Identity_Net8.HelperFunctions.ActionFilter;
using Identity_Net8.Models;
using Identity_Net8.Models.RequestResponses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using System.Net;

var builder = WebApplication.CreateBuilder(args);


// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();



// If we want to configure the Password field for the user Login. through identity

//builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
//{
//    opt.SignIn.RequireConfirmedAccount = false;
//    opt.Password.RequireNonAlphanumeric = false;
//    opt.Password.RequireDigit = false;
//    opt.Password.RequireLowercase = false;
//    opt.Password.RequireUppercase = false;
//}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();



// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// this will automatically execute while user will login and add identity 
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomClaimsPrincipalFactory>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ActiveRolePolicy", policy =>
    {
        policy.Requirements.Add(new ActiveRoleRequirement(true));
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, ActiveRoleHandler>();


//builder.Services.AddControllers();  // For only API contillers

builder.Services.AddControllersWithViews();  // for both APis and Web Pages controllers
//builder.Services.AddRazorPages();  // this is for enable RazorPages as well in your project

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure global exception handling
app.UseExceptionHandler(errorApp =>
{

    errorApp.Run(async context =>
    {

        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature?.Error;

        // Check if the request is for an API
        var isApiRequest = context.Request.Path.StartsWithSegments("/api") ||
                           context.Request.Headers["Accept"].ToString().Contains("application/json");

        if(exception is not null)
        {
            if(isApiRequest)
            {
                // Handle API exception
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new ApiResponse<object>
                {
                    Success = false,
                    Message = exception.Message,
                   
                });
            }
            else
            {
                // Handle non-API exception (e.g., for web pages)
                context.Response.ContentType = "text/html";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Redirect to an error page
                context.Response.Redirect("/Home/Error");
            }
        }
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();

// if you have only Web-Api then use this line of code.
//app.MapControllers();


// if you project contains web pages as well as PAI then these UseEndpoints route will use
app.UseEndpoints(endpoints =>
{

    // Defines a default route for MVC controllers.
    // This route pattern expects URLs in the format: /{controller}/{action}/{id?}.
    // The default controller is "Home" and the default action is "Index".
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
  
});

// API routes without session data
app.UseEndpoints(endpoints =>
{

    // Defines a separate route for API controllers.
    // This route pattern expects URLs in the format: /api/{controller}/{action}/{id?}.
    // This is useful for keeping API endpoints distinct from regular MVC routes.


    endpoints.MapControllerRoute(
        name: "api",
        pattern: "api/{controller}/{action}/{id?}");
});

//// Ensure the roles are created
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        var task = CreateRoles(services);
//        task.Wait();
//    }
//    catch (Exception ex)
//    {
//        // Log errors here or handle them as necessary
//        var logger = services.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "An error occurred while creating roles.");
//    }
//}

app.Run();



static async Task CreateRoles(IServiceProvider serviceProvider)
{
    //initializing custom roles 
    var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
    {
        if (context.Users.Any(s => s.Email == "superadmin@gmail.com") == false)
        {
            var user = new ApplicationUser { UserName = "superadmin@gmail.com", Email = "superadmin@gmail.com",  Hobby= "Admin!23" };
            var result = await UserManager.CreateAsync(user, "Admin!23");
            if (result.Succeeded)
            {
                await UserManager.AddToRoleAsync(user, "SuperAdmin");
            }
        }
    }
}