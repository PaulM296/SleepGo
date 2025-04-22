using Microsoft.EntityFrameworkCore;
using SleepGo.Api.Interfaces;
using SleepGo.Api.Services;
using SleepGo.App.Interfaces;
using SleepGo.Infrastructure;
using SleepGo.Infrastructure.Repositories;
using SleepGo.Infrastructure.Services;
using System.Runtime.CompilerServices;

namespace SleepGo.Api.Extensions
{
    public static class ServiceConfigurator
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IAmenityRepository, AmenityRepository>();
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
        }

        public static void AddMediatR(this IServiceCollection services)
        {
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IUserRepository).Assembly));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(IUserRepository).Assembly); // App
                cfg.RegisterServicesFromAssembly(typeof(SleepGo.Api.Features.Queries.GetUserRecommendationsQueryHandler).Assembly); // Api
            });
        }

        public static void AddDbContext(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext<SleepGoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void AddRecommendationService(this IServiceCollection services)
        {
            services.AddSingleton<IRecommendationService, RecommendationService>();
        }

        public static void AddOpenAI(this IServiceCollection services)
        {
            services.AddHttpClient<IOpenAIService, OpenAIService>();
        }
    }
}
