using NetProject.DTOs;
using NetProject.Models;
using Riok.Mapperly.Abstractions;

namespace NetProject.Mappers
{
    [Mapper]
    public static partial class ModelMapper
    {
        public static partial CustomerDTO ToCustomerDTO(Customer customer);
        public static partial Customer ToCustomer(CustomerDTO customerDto);

        public static partial VehicleDTO ToVehicleDTO(Vehicle vehicle);
        public static partial Vehicle ToVehicle(VehicleDTO vehicleDto);

        public static partial ServiceOrderDTO ToServiceOrderDTO(ServiceOrder order);
        public static partial ServiceOrder ToServiceOrder(ServiceOrderDTO orderDto);
    }
}