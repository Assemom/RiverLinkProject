using ArabRiver.Repository.Data;
using ArabRiver.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace ArabRiver.Api.Seed
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(
            ApplicationDbContext context)
        {
            // Check if admin already exists

            var adminExists =
                await context.Admins.AnyAsync();

            if (adminExists)
            {
                return;
            }

            // Create admin

            var admin = new Admin
            {
                Id = Guid.NewGuid(),

                Name = "Admin",

                Email = "admin@arabriver.com",

                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(
                        "Admin@123"),

                Role = "Admin",

                CreatedAt = DateTime.UtcNow
            };

            // Save admin

            await context.Admins.AddAsync(admin);

            await context.SaveChangesAsync();
        }
    }
}
