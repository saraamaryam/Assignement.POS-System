
using log4net;
using log4net.Config;
using Microsoft.EntityFrameworkCore;
using POS.API.Data;
using POS.API.Repositories.ProductRepository;
using POS.API.Repositories.Repository;
using POS.API.Services.ProductServices;
using System.Reflection;
using POS.API.Repositories.UserRepository;
using POS.API.Services.UserServices;
using POS.API.Services;
using POS.API.Repositories.TransactionRepository;
using POS.API.Services.TransactionServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

builder.Logging.ClearProviders();
builder.Logging.AddLog4Net();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(MapperConfig)); 


builder.Services.AddControllers();

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseInMemoryDatabase("Database"));

builder.Services.AddSingleton<TokenServices>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ProductService>();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<TransactionService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("6B29FC40-CA47-1067-B31D-00DD010662DA"))
            };
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CashierPolicy", policy => policy.RequireRole("Cashier"));
});
// Add Swagger generation
builder.Services.AddSwaggerGen();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    MyDbContext.SeedData(dbContext);
}


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
