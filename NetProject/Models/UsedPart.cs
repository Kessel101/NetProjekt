namespace NetProject.Models
{
    public class UsedPart
    {
        public int Id { get; set; }

        public int PartId { get; set; }
        public Part Part { get; set; }

        public int Quantity { get; set; }

        // Relacja z ServiceTask
        public int ServiceTaskId { get; set; }
        public ServiceTask ServiceTask { get; set; }
    }
}