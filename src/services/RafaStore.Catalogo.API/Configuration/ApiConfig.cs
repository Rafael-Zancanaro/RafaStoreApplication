﻿using Microsoft.EntityFrameworkCore;
using RafaStore.Catalogo.API.Data;
using RafaStore.WebAPI.Core.Identidade;

namespace RafaStore.Catalogo.API.Configuration;

public static class ApiConfig
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogoContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("Total", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("Total");

        app.UseJwtConfiguration();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}