using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NetProject.Controllers;
using NetProject.Data;
using NetProject.Models;
using NetProject.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetProject.Tests.Controllers
{
    public class CustomersControllerTests
    {
        private List<Customer> GetTestCustomers() =>
            new()
            {
                new Customer { Id = 1, FullName = "Anna Nowak", Email = "anna@example.com" },
                new Customer { Id = 2, FullName = "Jan Kowalski", Email = "jan@example.com" }
            };

        private DbContextOptions<MyAppDbContext> CreateInMemoryDbOptions()
        {
            return new DbContextOptionsBuilder<MyAppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;
        }

        [Fact]
        public async Task Index_ReturnsViewWithCustomers()
        {
            var options = CreateInMemoryDbOptions();
            await using var context = new MyAppDbContext(options);
            context.Customers.AddRange(GetTestCustomers());
            await context.SaveChangesAsync();

            var controller = new CustomersController(context);
            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Customer>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Details_ValidId_ReturnsCustomerWithVehicles()
        {
            var options = CreateInMemoryDbOptions();
            await using var context = new MyAppDbContext(options);
            context.Customers.Add(new Customer
            {
                Id = 1,
                FullName = "Test",
                Vehicles = new List<Vehicle> { new Vehicle { Model = "Toyota" } }
            });
            await context.SaveChangesAsync();

            var controller = new CustomersController(context);
            var result = await controller.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var customer = Assert.IsType<Customer>(viewResult.Model);
            Assert.Single(customer.Vehicles);
        }

        [Fact]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            var options = CreateInMemoryDbOptions();
            await using var context = new MyAppDbContext(options);
            var controller = new CustomersController(context);

            var viewModel = new CustomerViewModel
            {
                FullName = "Test User",
                Email = "test@example.com",
                PhoneNumber = "123456789"
            };

            var result = await controller.Create(viewModel);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            Assert.Single(context.Customers);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesCustomerAndRedirects()
        {
            var options = CreateInMemoryDbOptions();
            await using var context = new MyAppDbContext(options);
            context.Customers.Add(new Customer { Id = 5, FullName = "To Delete" });
            await context.SaveChangesAsync();

            var controller = new CustomersController(context);
            var result = await controller.DeleteConfirmed(5);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Empty(context.Customers);
        }
    }
}
