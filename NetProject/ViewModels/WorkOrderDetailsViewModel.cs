using System.Collections.Generic;
using System.Linq;
using NetProject.Models;

namespace NetProject.ViewModels
{
    public class WorkOrderDetailsViewModel
    {
        public WorkOrder WorkOrder { get; set; } = default!;
        public List<ServiceTask> ServiceTasks { get; set; } = new();

        public decimal TotalLaborCost => ServiceTasks.Sum(t => t.LaborCost);

        public decimal TotalPartsCost => ServiceTasks
            .SelectMany(t => t.UsedParts ?? new List<UsedPart>())
            .Sum(up => up.Quantity * (up.Part?.UnitPrice ?? 0));

        public decimal TotalCost => TotalLaborCost + TotalPartsCost;
    }
}