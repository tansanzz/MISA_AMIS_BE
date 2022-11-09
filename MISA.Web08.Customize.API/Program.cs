using System.Buffers.Text;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MISA.Web08.Customize.BL;
using MISA.Web08.Customize.Common.Resources;
using MISA.Web08.Customize.DL;
using MISA.Web08.Customize.DL.MySql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
     options.AddDefaultPolicy(policy =>
     {
          policy.WithOrigins("*");
          policy.AllowAnyHeader();
          policy.AllowAnyMethod();
     }
     );
}
);

// Naming field to PascalCase
builder.Services.AddControllers().AddJsonOptions((jsonOptions) =>
{
     jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
}
);

// JwtBearerAuthentication middleware
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
     option.TokenValidationParameters = new TokenValidationParameters
     {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[Resource.AuthSettings_Key])),
          ValidateAudience = true,
          ValidAudience = builder.Configuration[Resource.AuthSettings_Audience],
          ValidateIssuer = true,
          ValidIssuer = builder.Configuration[Resource.AuthSettings_Issuer],
          RequireExpirationTime = true,
          ValidateLifetime = true,
          ClockSkew = TimeSpan.Zero
     };
});

// Get connection string
DataContext.MySqlConnectionString = builder.Configuration.GetConnectionString("MySqlConnectionString");

// Dependency Injection
builder.Services.AddDapperMySql(DataContext.MySqlConnectionString);
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddScoped<IEmployeeBL, EmployeeBL>();
builder.Services.AddScoped<IEmployeeDL, EmployeeDL>();
builder.Services.AddScoped<IDepartmentDL, DepartmentDL>();
builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();


builder.Services.AddControllers().ConfigureApiBehaviorOptions(option =>
{
     option.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
     builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
     app.UseSwaggerUI();
}

//app cors
app.UseCors("corsapp");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
