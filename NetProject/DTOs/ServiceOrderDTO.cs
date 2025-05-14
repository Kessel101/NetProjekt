namespace NetProject.DTOs;

public class ServiceOrderDTO
{
    public string Status { get; set; }
    public string AssignedMechanic { get; set; }
    public List<ServiceTaskDTO> ServiceTasks { get; set; }
    public List<CommentDTO> Comments { get; set; }
}