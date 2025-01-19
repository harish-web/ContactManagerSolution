using ContacManager.Core.Domain.IdentityEntities;
using ContacManager.Core.Domain.RepositoryContracts;
using ContacManager.Core.Servicecontracts;
using ContacManager.Core.Services;
using ContactManager.Infrastructure.DbContext;
using ContactManager.Infrastructure.Repository;
using CRUDOperation.Filters.ActionFilter;
using CRUDOperation.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
       
        /*var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
        {
            WebRootPath = "webroot" // custom folder
        });*/
        //we cannot add argument to filter if we follow this
        //builder.Services.AddControllersWithViews(options => options.Filters.Add<ResponseHeaderActionfilter>(5));5 is order num

        //var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionfilter>>();
        //var logger = IServiceProvider.GetRequiredService<ILogger<ResponseHeaderActionfilter>>();

        //Global filter registrtion
        builder.Services.AddControllersWithViews(options =>
        {
           // options.Filters.Add(new ResponseHeaderActionfilter(logger, "My-Global-key","My-Gloabl-Value",2));
            //options.Filters.Add(new ValidateAntiForgeryTokenAttribute());//this will apply set and get
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());//For HttpPost unsafe method

        });
        //builder.Services.Configure<MvcOptions>(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

        builder.Services.AddScoped<ICountriesUploadFromExcelService, CountriesUploadFromExcelService>();
        builder.Services.AddScoped<ICountriesAdderService, CountriesAdderService>();
        builder.Services.AddScoped<ICountriesGetterService, CountriesGetterService>();
        builder.Services.AddScoped<IPersonsAdderService, PersonsAdderService>();
        builder.Services.AddScoped<IPersonsGetterService, PersonsGetterService>();
        builder.Services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
        builder.Services.AddScoped<IPersonsSorterService, PersonsSorterService>();
        builder.Services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
      
        builder.Services.AddScoped<ICountriesRepository,EntityCoreCountriesRepository>();
        builder.Services.AddScoped<IPersonsRepository, EntityCorePersonsRepository>();
        // builder.Services.AddScoped<PersonsListActionFilter>();

        //builder.Host.ConfigureLogging(logProvider => 
        //                               logProvider.AddConsole()
        //                                          .AddDebug()
        //                                          .AddEventLog());
        builder.Logging.AddDebug().AddConsole();
        builder.Services.AddHttpLogging(option =>
        {
            option.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders
            | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders;
        }

        );


        //builder.Services.AddDbContext<ApplicationDbContext>(option=> ConfigureDb(option,builder.Services.AddDbContext<ApplicationDbContext>(option=> ConfigureDb(option,builder)) )) ;
        builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("PersonsDBConnection")));
        //builder.Services.Configure<DbContextOptionsBuilder>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("PersonsDBConnection")));
        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequiredLength = 2;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = true;
            options.Password.RequiredUniqueChars = 1;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>().AddDefaultTokenProviders();

        builder.Services.Configure< IdentityOptions>(options => options.Password.RequiredUniqueChars = 2);
        

        builder.Services.AddAuthorization(options =>
        {
            //enforce user must be authenticate for all    action method in entire application simply it is
            //authorization filter for all method
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            options.FallbackPolicy = policy;

           

            //options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));
            //options.AddPolicy("LoginPolicy", policy);
            options.AddPolicy("NotLoggedIn", policy => policy.RequireAssertion(context => !context.User.Identity.IsAuthenticated));
        });
        builder.Services.Configure<AuthorizationOptions>(options => options.AddPolicy("NotAuthenticated",
            policy => policy.RequireAssertion(context => !context.User.Identity.IsAuthenticated)));
        //
       builder.Services.ConfigureApplicationCookie(options =>
        {
            //if action name is login it will automatically redirect if some other name explicitly  mention here
            
            options.LoginPath = "/Account/SignIn";//default path /Account/Login
            //options.AccessDeniedPath = new PathString("/Administration/AccessDenied");//default path /Account/AccessDenied
        });
        
       

        var app = builder.Build();
       
       // string countriesJson = System.IO.File.ReadAllText("countries.json");

       // List<Country>? countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseStatusCodePagesWithRedirects("/Error/{0}");
            //app.UseExceptionHandler();
            app.UseExceptionHandlingMiddleware();
        }
        Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseHttpLogging();
        app.UseStaticFiles();
        //app.UseRouting(); //Identifying action method based on route
        //app.UseAuthentication(); //Reading Identity cookie
        app.UseAuthorization(); //Validates access permissions of the user
        app.MapControllers(); //Execute the filter pipiline (action + filters)
        app.MapControllerRoute(
           name: "areas",
           pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=persons}/{action=index}/{id?}",
            defaults:new  { controller = "persons", action = "index" });


       


        app.Run();
    }

   /* private static void ConfigureDb(DbContextOptionsBuilder option, WebApplicationBuilder builder)
    {
        option.UseSqlServer(builder.Configuration.GetConnectionString("PersonsDBConnection"));
                               
    }*/
}