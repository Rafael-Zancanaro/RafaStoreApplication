using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RafaStore.Identidade.API.Data;
using RafaStore.Identidade.API.Extensions;
using RafaStore.WebAPI.Core.Identidade;

namespace RafaStore.Identidade.API.Configuration;

public static class IdentityConfig
{
    public static void AddIdentityConfiguration(this IServiceCollection services,
                                                              IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddErrorDescriber<IdentityMensagensPortugues>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddJwtConfiguration(configuration);
    }
}
