using System.Text;
using ApiEcommerce.Constants;
using ApiEcommerce.Data;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos.Product;
using ApiEcommerce.Repository;
using ApiEcommerce.Repository.IRepository;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Mapster;
using DotNetEnv;

// Cargar variables de entorno desde .env
Env.Load(".env");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Leer la cadena de conexión desde variable de entorno
var dbConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
    ?? throw new InvalidOperationException("La variable de entorno CONNECTION_STRING no está configurada");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
   options.UseSqlServer(dbConnectionString)
   .UseSeeding((context, _) =>
   {
      var appContext = (ApplicationDbContext)context;
      DataSeeder.SeedData(appContext);
   });
});

builder.Services.AddResponseCaching(options =>
{
   // El tamaño máximo que guarda (1MB)
   options.MaximumBodySize = 1024 * 1024;
   options.UseCaseSensitivePaths = true;
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Configure Mapster mappings
TypeAdapterConfig<Product, ProductDto>.NewConfig()
    .Map(dest => dest.CategoryName, src => src.Category.Name);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY")
    ?? throw new InvalidOperationException("La variable de entorno SECRET_KEY no está configurada");
if (string.IsNullOrWhiteSpace(secretKey)) throw new InvalidOperationException("La secret key no está configurada");

builder.Services.AddAuthentication(options =>
{
   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
   // Para desplegar en producción se debe usar RequireHttpsMetadata en true
   // Necesita obligatoriamente que las peticiones sean HTTPS
   options.RequireHttpsMetadata = false;

   options.SaveToken = true;
   options.TokenValidationParameters = new TokenValidationParameters
   {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
      ValidateIssuer = false,
      ValidateAudience = false
   };
});

builder.Services.AddControllers(options =>
{
   options.CacheProfiles.Add(CacheProfiles.Default10, CacheProfiles.Profile10);
   options.CacheProfiles.Add(CacheProfiles.Default20, CacheProfiles.Profile20);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(
  options =>
  {
     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
     {
        Description = "Nuestra API utiliza la Autenticación JWT usando el esquema Bearer. \n\r\n\r" +
                     "Ingresa la clave a continuación con token generado en login.\n\r\n\r" +
                     "Ejemplo: \"ey12345abcde\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
     });
     options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
     {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
     });
     options.SwaggerDoc("v1", new OpenApiInfo
     {
        Version = "v1",
        Title = "ApiEcommerce",
        Description = "API para gestionar productos y usuarios",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
           Name = "Keyner de Ávila",
           Url = new Uri("https://example.com/contact"),
        },
        License = new OpenApiLicense
        {
           Name = "Licencia de uso",
           Url = new Uri("https://example.com/license")
        }
     });
     options.SwaggerDoc("v2", new OpenApiInfo
     {
        Version = "v2",
        Title = "ApiEcommerce",
        Description = "API para gestionar productos y usuarios",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact { Name = "Keyner de Ávila", Url = new Uri("https://example.com/contact") },
        License = new OpenApiLicense { Name = "Licencia de uso", Url = new Uri("https://example.com/license") }
     });
  }
);

var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
{
   options.AssumeDefaultVersionWhenUnspecified = true;
   options.DefaultApiVersion = new ApiVersion(1, 0);
   options.ReportApiVersions = true;
});
apiVersioningBuilder.AddApiExplorer(options =>
{
   options.GroupNameFormat = "'v'VVV"; // v1,v2,v3...
   options.SubstituteApiVersionInUrl = true; // api/v{version}/products
});

builder.Services.AddCors(options =>
{
   options.AddPolicy(PolicyNames.AllowSpecificOrigin,
   builder =>
   {
      builder
      .WithOrigins("https://localhost:3000")
      .AllowAnyMethod()
      .AllowAnyHeader();
   }
   );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.MapOpenApi();
   app.UseSwagger();
   app.UseSwaggerUI(options =>
   {
      options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
      options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
   });
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors(PolicyNames.AllowSpecificOrigin);

app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
