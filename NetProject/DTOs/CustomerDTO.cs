namespace NetProject.DTOs;

public class CustomerDTO
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public List<VehicleDTO> Vehicles { get; set; }
}