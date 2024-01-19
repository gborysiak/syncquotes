using CoinMarketOdata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using StoreCoinMarket;
using NLog;
using NLog.Web;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

Settings _settings = singletonSettings.Instance.GetSettings();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


    ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
    //modelBuilder.EntityType<CoinHistory>();
    modelBuilder.ComplexType<CoinHistory>();
    modelBuilder.EntitySet<Coin>("Coin");

    builder.Services.AddControllers().AddOData(
        options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100).AddRouteComponents(
            "odata",
            modelBuilder.GetEdmModel()));


    // Add services to the container.
    //builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    //builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<CoinDbContext>(options =>
    {
        var str = $"Server={_settings.MariaDbHost};" +
                         $"Database={_settings.MariaDbDatabase};" +
                         $"Uid={_settings.MariaDbUser};" +
                         $"Pwd={_settings.MariaDbDPassword};" +
                         "Pooling=true;";
        options.UseMySql(str
            , ServerVersion.AutoDetect(str)
        );
    });

    /*
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            //var key = new SymmetricSecurityKey(securityService.Key);
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                //IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };
        });
    */

    var app = builder.Build();



    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseAuthentication();


    //app.UseHttpsRedirection();

    

    //app.MapControllers();

    app.UseRouting();
    //app.UseAuthorization();
    app.UseEndpoints(endpoints => endpoints.MapControllers());

    //Console.WriteLine("token " + generateJwt(builder.Configuration));

    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}


/*
 string generateJwt(ConfigurationManager _config)
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new[] {
            new Claim("role","admin"),
            new Claim(ClaimTypes.NameIdentifier,"admin")
        };

    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        _config["Jwt:Issuer"],
        claims,
        expires: DateTime.Now.AddMinutes(120),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
}
*/