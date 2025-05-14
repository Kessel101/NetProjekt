using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetProject.Models;
using NetProject.Mappers;
using NetProject.DTOs;

namespace NetProject.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Mechanik", "Recepcjonista" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Tworzenie domyślnego Admina
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser { UserName = adminEmail, Email = adminEmail, FullName = "Admin Admin" };
                var result = await userManager.CreateAsync(user, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
        
        /// <summary>
        /// Metoda do seedowania przykładowych danych warsztatowych.
        /// </summary>
        public static async Task SeedSampleDataAsync(MyAppDbContext context)
        {
            if (await context.Customers.AnyAsync()) return; // Zapobiegamy powielaniu danych

            var customer = new Customer
            {
                FullName = "Jan Nowak",
                Email = "jan.nowak@example.com",
                PhoneNumber = "555-123-456"
            };

            var vehicle = new Vehicle
            {
                Make = "BMW",
                Model = "X5",
                VIN = "BMW123456789",
                RegistrationNumber = "KR1234",
                ImageUrl = "/uploads/bmw-x5.jpg",
                Customer = customer
            };

            var part = new Part
            {
                Name = "Klocki hamulcowe",
                UnitPrice = 300
            };

            var serviceOrder = new ServiceOrder
            {
                Vehicle = vehicle,
                Status = "Nowe",
                ServiceTasks = new List<ServiceTask>
                {
                    new ServiceTask
                    {
                        Description = "Wymiana klocków hamulcowych",
                        LaborCost = 200,
                        UsedParts = new List<UsedPart>
                        {
                            new UsedPart
                            {
                                Part = part,
                                Quantity = 1
                            }
                        }
                    }
                },
                Comments = new List<Comment>
                {
                    new Comment
                    {
                        Content = "Klient prosił o szybką obsługę",
                        AuthorId = null
                    }
                }
            };

            await context.Customers.AddAsync(customer);
            await context.Parts.AddAsync(part);
            await context.ServiceOrders.AddAsync(serviceOrder);

            await context.SaveChangesAsync();
        }
    }
}