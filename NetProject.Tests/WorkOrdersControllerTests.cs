using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using NetProject.Controllers;
using NetProject.Data;
using NetProject.Models;
using NetProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace NetProject.Tests
{
    public class WorkOrdersControllerTests : IDisposable
    {
        private readonly MyAppDbContext _db;
        private readonly WorkOrdersController _controller;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkOrdersControllerTests()
        {
            var options = new DbContextOptionsBuilder<MyAppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _db = new MyAppDbContext(options);

            // Seed sample data
            _db.Customers.Add(new Customer { Id = 1, FullName = "Test", Email = "t@test", PhoneNumber = "123" });
            _db.Vehicles.Add(new Vehicle { Id = 1, Make = "M", Model = "Mo", VIN = "V1", RegistrationNumber = "R1", Year = 2020, CustomerId = 1 });
            _db.Users.Add(new ApplicationUser { Id = "u1", UserName = "mech@test" });
            _db.SaveChanges();

            // Mock UserManager
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null).Object;

            _controller = new WorkOrdersController(_db, _userManager);
        }

        [Fact]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var model = new WorkOrderViewModel
            {
                CustomerId = 1,
                VehicleId = 1,
                AssignedMechanicId = "u1",
                Description = "Desc",
                Mechanics = new List<ApplicationUser> { new ApplicationUser { Id = "u1", UserName = "mech" } },
                Customers = _db.Customers.ToList(),
                Vehicles = _db.Vehicles.ToList()
            };

            // Act
            var result = await _controller.Create(model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal(1, _db.WorkOrders.Count());
        }

        [Fact]
        public async Task Details_ExistingId_ReturnsViewWithModel()
        {
            // Arrange: create an order
            var order = new WorkOrder { Id = 10, CustomerId = 1, VehicleId = 1, AssignedMechanicId = "u1", Description = "desc", Status = "Nowe", CreatedAt = DateTime.UtcNow };
            _db.WorkOrders.Add(order);
            _db.SaveChanges();

            // Act
            var result = await _controller.Details(10) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<WorkOrder>(result.Model);
            Assert.Equal(10, model.Id);
        }

        public void Dispose()
        {
            _db.Database.EnsureDeleted();
            _db.Dispose();
        }
    }
}
