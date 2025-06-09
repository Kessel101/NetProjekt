using System;
using System.Collections.Generic;
using System.Linq;

namespace NetProject.ViewModels
{
    public class MonthlySummaryReportViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }

        // zestawienie po kliencie
        public List<MonthlySummaryItem> Items { get; set; } = new();

        public decimal GrandTotalCost => Items.Sum(i => i.TotalCost);
        public int TotalOrders     => Items.Sum(i => i.OrderCount);
    }

    public class MonthlySummaryItem
    {
        public string CustomerName { get; set; }
        public string VehicleReg   { get; set; }
        public int OrderCount      { get; set; }
        public decimal TotalCost   { get; set; }
    }
}