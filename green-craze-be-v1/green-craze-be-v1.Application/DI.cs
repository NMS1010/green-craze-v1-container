using FluentValidation;
using FluentValidation.AspNetCore;
using green_craze_be_v1.Application.Common.Extensions;
using green_craze_be_v1.Application.Common.Mapper;
using green_craze_be_v1.Application.Common.Options;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Services;
using Hangfire;
using Hangfire.MySql;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

namespace green_craze_be_v1.Application
{
    public static class DI
    {

        private static string GetHangFireConnectionString(IConfiguration configuration)
        {
            var connectionStr = configuration.GetConnectionString("HangfireDB");
            MySqlConnectionStringBuilder builder = new(connectionStr);
            builder.ConnectionString = connectionStr;

            var databaseName = builder.Database;
            builder.Database = "sys";

            using var connection = new MySqlConnection(builder.ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "CREATE DATABASE IF NOT EXISTS " + databaseName;
            command.ExecuteNonQuery();

            return connectionStr;
        }

        [Obsolete]
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMapper();
            services.AddProblemDetailsSetup();
            services.AddControllers(options =>
                {
                    options.SuppressAsyncSuffixInActionNames = false;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var listViolation = new List<green_craze_be_v1.Application.Model.CustomAPI.APIViolation>();
                        context.ModelState.Keys.ToList().ForEach(x =>
                        {
                            listViolation.Add(new green_craze_be_v1.Application.Model.CustomAPI.APIViolation()
                            {
                                Field = x,
                                Messages = context.ModelState[x]?.Errors.Select(e => e.ErrorMessage).ToList()
                            });
                        });
                        return new BadRequestObjectResult(new green_craze_be_v1.Application.Model.CustomAPI.ErrorResponse()
                        {
                            Violations = listViolation,
                            Title = "One or more validation errors occurred",
                            TraceId = context.HttpContext.TraceIdentifier,
                            Instance = context.HttpContext.Request.Path,
                            Status = 400,
                            Type = new Uri("https://tools.ietf.org/html/rfc7231#section-6.5.1"),
                            Code = "VALIDATION_ERROR",
                        });
                    };
                })
                .AddFluentValidation(v =>
                {
                    v.ImplicitlyValidateChildProperties = true;
                    v.ImplicitlyValidateRootCollectionElements = true;
                    v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });
            services.AddValidators();
            services.AddServices();
            services.AddSwaggerGenWithJWTAuth();

            var connectionString = GetHangFireConnectionString(configuration);
            services.AddHangfire(cfg => cfg
                   .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                   .UseSimpleAssemblyNameTypeSerializer()
                   .UseRecommendedSerializerSettings()
                   .UseStorage(
                        new MySqlStorage(
                            connectionString,
                            new MySqlStorageOptions
                            {
                                QueuePollInterval = TimeSpan.FromSeconds(10),
                                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                                PrepareSchemaIfNecessary = true,
                                DashboardJobListLimit = 25000,
                                TransactionTimeout = TimeSpan.FromMinutes(1),
                                TablesPrefix = "Hangfire",
                            }
                        )
                    ));
            services.AddHangfireServer();
        }

        private static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile).Assembly);
        }

        private static void AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private static void AddProblemDetailsSetup(this IServiceCollection services)
        {
            services.AddProblemDetails(setup =>
            {
                setup.IncludeExceptionDetails = (ctx, env) =>
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging";
            });
        }

        private static JWTConfigOptions GetJwtConfig(IConfiguration configuration)
        {
            return configuration.GetOptions<JWTConfigOptions>("Tokens");
        }

        private static void AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddScoped<IJwtService, JwtService>()
                .AddScoped<IDateTimeService, DateTimeService>()
                .AddScoped<ITokenService, TokenService>()
                .AddScoped<IBackgroundJobService, BackgroundJobService>()
                .AddScoped<IUnitService, UnitService>()
                .AddScoped<INotificationService, NotificationService>()
                .AddScoped<IBrandService, BrandService>()
                .AddScoped<IVariantService, VariantService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ICartService, CartService>()
                .AddScoped<IDeliveryService, DeliveryService>()
                .AddScoped<IPaymentMethodService, PaymentMethodService>()
                .AddScoped<IOrderCancellationReasonService, OrderCancellationReasonService>()
                .AddScoped<IUserFollowProductService, UserFollowProductService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<IUnitService, UnitService>()
                .AddScoped<IProductCategoryService, ProductCategoryService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IProductImageService, ProductImageService>()
                .AddScoped<ISaleService, SaleService>()
                .AddScoped<IAddressService, AddressService>()
                .AddScoped<IUnitService, UnitService>()
                .AddScoped<ITransactionService, TransactionService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IReviewService, ReviewService>()
                .AddScoped<IInventoryService, InventoryService>()
                .AddScoped<IReviewService, ReviewService>()
                .AddScoped<IStatisticService, StatisticService>();
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = GetJwtConfig(configuration);
            byte[] signingKeyBytes = Encoding.UTF8.GetBytes(jwtConfig.SigningKey);
            services
                .AddAuthentication(opts =>
                {
                    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opts =>
                {
                    opts.RequireHttpsMetadata = false;
                    opts.SaveToken = true;
                    opts.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtConfig.Issuer,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                    };
                    opts.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/app-hub"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }

    }

    static class SwaggerAuthExtension
    {
        private static readonly string AUTH_TYPE = "Bearer";

        private static readonly OpenApiSecurityRequirement requirement = new()
        {{
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = AUTH_TYPE
                },
                Scheme = "oauth2",
                Name = AUTH_TYPE,
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }};

        private static readonly OpenApiSecurityScheme scheme = new()
        {
            Description = @"JWT authorization header using the Bearer sheme. \r\n\r\n
                        Enter 'Bearer' [space] and then your token in the text input below.
                        \r\n\r\nExample: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = AUTH_TYPE
        };

        private static void AddJWTAuth(this SwaggerGenOptions option)
        {
            option.AddSecurityDefinition(AUTH_TYPE, scheme);
            option.OperationFilter<SecurityRequirementsOperationFilter>();
        }

        public static void AddSwaggerGenWithJWTAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt => opt.AddJWTAuth());
        }

        private class SecurityRequirementsOperationFilter : IOperationFilter
        {
            private static bool HasAttribute(MethodInfo methodInfo, Type type, bool inherit)
            {
                var actionAttributes = methodInfo.GetCustomAttributes(inherit);
                var controllerAttributes = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(inherit);
                var actionAndControllerAttributes = actionAttributes.Union(controllerAttributes);

                return actionAndControllerAttributes.Any(attr => attr.GetType() == type);
            }
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                bool hasAuthorizeAttribute = HasAttribute(context.MethodInfo, typeof(AuthorizeAttribute), true);
                bool hasAnonymousAttribute = HasAttribute(context.MethodInfo, typeof(AllowAnonymousAttribute), true);

                bool isAuthorized = hasAuthorizeAttribute && !hasAnonymousAttribute;
                if (isAuthorized)
                {
                    operation.Security = new List<OpenApiSecurityRequirement>
                    {
                        requirement
                    };
                }
            }
        }
    }
}