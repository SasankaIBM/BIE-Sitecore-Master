namespace CloudCostOptimizer.Models
{
    public class CloudResource
    {
        public string ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string Provider { get; set; }
        public ResourceType Type { get; set; }
        public ResourceState State { get; set; }
        public string Region { get; set; }
        public decimal MonthlyCost { get; set; }
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
        public double StorageUsage { get; set; }
        public int DaysIdle { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastAccessDate { get; set; }
        public string InstanceSize { get; set; }
        public string RecommendedSize { get; set; }
        public decimal EstimatedWaste { get; set; }
        public decimal PotentialSavings { get; set; }
        public List<string> Tags { get; set; }
        public bool IsProduction { get; set; }

        public CloudResource()
        {
            Tags = new List<string>();
        }
    }
}

// Made with Bob
