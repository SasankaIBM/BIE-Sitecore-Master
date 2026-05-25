namespace CloudCostOptimizer.Models
{
    public class IdleResource
    {
        public string ResourceName { get; set; }
        public string Type { get; set; }
        public decimal EstimatedWaste { get; set; }
        public double CpuUsage { get; set; }
    }
}
