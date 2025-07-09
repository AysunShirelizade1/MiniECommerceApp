using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence;
using MiniECommerce.Persistence.Contexts;
using MiniECommerce.Persistence.Helpers;
using MiniECommerce.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

//  Custom service-lərin qeydiyyatı
builder.Services.RegisterService(builder.Configuration);

// ✅DbContext qeydiyyatı
builder.Services.AddDbContext<MiniECommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

//  Identity qeydiyyatı
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<MiniECommerceDbContext>()
.AddDefaultTokenProviders();

// JWT Token Service qeydiyyatı
builder.Services.AddScoped<JwtTokenService>();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        RoleClaimType = ClaimTypes.Role
    };
});

//  Permission-based Policy Authorization
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddAuthorization(options =>
{
    // Category icazələri
    options.AddPolicy("Category.Read", policy =>
        policy.Requirements.Add(new PermissionRequirement("Category.Read")));
    options.AddPolicy("Category.Create", policy =>
        policy.Requirements.Add(new PermissionRequirement("Category.Create")));
    options.AddPolicy("Category.Update", policy =>
        policy.Requirements.Add(new PermissionRequirement("Category.Update")));
    options.AddPolicy("Category.Delete", policy =>
        policy.Requirements.Add(new PermissionRequirement("Category.Delete")));

    //  Product icazələri
    options.AddPolicy("Product.Read", policy =>
        policy.Requirements.Add(new PermissionRequirement("Product.Read")));
    options.AddPolicy("Product.Create", policy =>
    policy.Requirements.Add(new PermissionRequirement("Product.Create")));

    options.AddPolicy("Product.Update", policy =>
        policy.Requirements.Add(new PermissionRequirement("Product.Update")));

    options.AddPolicy("Product.Delete", policy =>
        policy.Requirements.Add(new PermissionRequirement("Product.Delete")));

    // Email göndərmə icazəsi

    options.AddPolicy("SendTestEmail", policy =>
    policy.RequireClaim(ClaimTypes.Role, "Admin")); // Yaxud uyğun rolu qoy



    // Order icazələri
    options.AddPolicy("Order.Read", policy =>
    policy.Requirements.Add(new PermissionRequirement("Order.Read")));

    options.AddPolicy("Order.Create", policy =>
        policy.Requirements.Add(new PermissionRequirement("Order.Create")));

    options.AddPolicy("Order.Update", policy =>
        policy.Requirements.Add(new PermissionRequirement("Order.Update")));
    options.AddPolicy("Order.Cancel", policy =>
    policy.Requirements.Add(new PermissionRequirement("Order.Cancel")));



    // Image icazələri
    options.AddPolicy("Image.Create", policy =>
        policy.Requirements.Add(new PermissionRequirement("Image.Create")));
    options.AddPolicy("Image.Update", policy =>
        policy.Requirements.Add(new PermissionRequirement("Image.Update")));
    options.AddPolicy("Image.Delete", policy =>
        policy.Requirements.Add(new PermissionRequirement("Image.Delete")));

    // Review icazələri
    options.AddPolicy("Review.Create", policy =>
    policy.Requirements.Add(new PermissionRequirement("Review.Create")));

    options.AddPolicy("Review.Delete", policy =>
        policy.Requirements.Add(new PermissionRequirement("Review.Delete")));


    //  User və Role idarəsi
    options.AddPolicy("User.Manage", policy =>
        policy.Requirements.Add(new PermissionRequirement("User.Manage")));
    options.AddPolicy("Role.Manage", policy =>
        policy.Requirements.Add(new PermissionRequirement("Role.Manage")));

    // Statistikalar və idarə paneli
    options.AddPolicy("Analytics.View", policy =>
        policy.Requirements.Add(new PermissionRequirement("Analytics.View")));
});


builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mini E-Commerce API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT formatında token daxil edin. Məsələn: Bearer eyJhbGciOi..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();
//// DB initializasiyası
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MiniECommerceDbContext>();
    await DbInitializer.InitializeAsync(context);
}

//  Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
