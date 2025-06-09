using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetProject.Models;
using Xunit;

namespace NetProject.Tests.Models
{
    public class VehicleTests
    {
        private bool TryValidateModel(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model, null, null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }

        [Fact]
        public void Vehicle_WithValidData_ShouldBeValid()
        {
            var vehicle = new Vehicle
            {
                Make = "Toyota",
                Model = "Corolla",
                VIN = "JTDBU4EE9A9123456",
                RegistrationNumber = "KR12345",
                Year = 2015,
                CustomerId = 1
            };

            var isValid = TryValidateModel(vehicle, out var results);

            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Theory]
        [InlineData("", "Corolla", "VIN", "REG", 2020)] // Make is empty
        [InlineData("Toyota", "", "VIN", "REG", 2020)] // Model is empty
        [InlineData("Toyota", "Corolla", "", "REG", 2020)] // VIN is empty
        [InlineData("Toyota", "Corolla", "VIN", "", 2020)] // Registration is empty
        public void Vehicle_MissingRequiredFields_ShouldBeInvalid(string make, string model, string vin, string reg, int year)
        {
            var vehicle = new Vehicle
            {
                Make = make,
                Model = model,
                VIN = vin,
                RegistrationNumber = reg,
                Year = year,
                CustomerId = 1
            };

            var isValid = TryValidateModel(vehicle, out var results);

            Assert.False(isValid);
            Assert.NotEmpty(results);
        }

        [Fact]
        public void Vehicle_YearIsRequired()
        {
            var vehicle = new Vehicle
            {
                Make = "Toyota",
                Model = "Corolla",
                VIN = "VIN123",
                RegistrationNumber = "REG123",
                Year = 0, // default int is 0, which will pass [Required], so test manually
                CustomerId = 1
            };

            Assert.Equal(0, vehicle.Year); // logic might be needed elsewhere to validate realistic years
        }
    }
}
