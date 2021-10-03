using System;
using System.Collections.Generic;
using CommandsService.Models;
using CommandsService.Services.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.Data
{
  public static class PrepDb
  {
    public static void PrepPopulation(IApplicationBuilder applicationBuilder, bool isProduction)
    {
      using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
      {
        var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

        var platforms = grpcClient.ReturnAllPlatforms();

        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms, isProduction);
      }
    }

    private static void SeedData(AppDbContext context, ICommandRepo repo, IEnumerable<Platform> platforms, bool isProduction)
    {

      if (isProduction)
      {
        Console.WriteLine("--> Trying migrations...");
        try
        {
          context.Database.Migrate();
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Could not run migrations {ex.Message}");
        }
      }

      Console.WriteLine($"--> Seeding existing platforms");

      foreach (var platform in platforms)
      {
        if (!repo.ExternalPlatformExists(platform.ExternalId))
        {
          repo.CreatePlatform(platform);
        }

        repo.SaveChanges();
      }
    }
  }
}