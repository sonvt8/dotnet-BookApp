using api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context, UserManager<AppUsers> userManager, RoleManager<AppRole> roleManager)
        {
            // Seed Categories
            if (await context.Categories.AnyAsync())
                return;
            var categoryData = await System.IO.File.ReadAllTextAsync("Data/CategorySeedData.json");
            var categories = JsonSerializer.Deserialize<List<Category>>(categoryData);
            foreach (var category in categories)
            {
                context.Categories.Add(category);
            }
            await context.SaveChangesAsync();

            // Seed Users
            if (await userManager.Users.AnyAsync())
                return;
            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUsers>>(userData);

            if (users == null) return;

            var roles = new List<AppRole>
            {
                new AppRole {Name = "Member"},
                new AppRole {Name = "Admin"},
                new AppRole {Name = "Moderator"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "password");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUsers
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "password");
            await userManager.AddToRoleAsync(admin, "Admin");
            await userManager.AddToRoleAsync(admin, "Moderator");
        }
    }
}
