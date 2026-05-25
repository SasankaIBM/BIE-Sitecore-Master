namespace CloudCostOptimizer.Models
{
    public class Recommendation
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public RecommendationPriority Priority { get; set; }
        public WasteCategory Category { get; set; }
        public decimal PotentialMonthlySavings { get; set; }
        public decimal AnnualSavings { get; set; }
        public int ImplementationEffort { get; set; } // 1-5 scale
        public double ImpactPercentage { get; set; }
        public string ActionRequired { get; set; }
        public List<string> AffectedResources { get; set; }
        public int ResourceCount { get; set; }
        public string Provider { get; set; }
        public DateTime IdentifiedDate { get; set; }

        public Recommendation()
        {
            AffectedResources = new List<string>();
            IdentifiedDate = DateTime.UtcNow;
        }

        public double CalculateROI()
        {
            if (ImplementationEffort == 0) return 0;
            return (double)AnnualSavings / (ImplementationEffort * 100);
        }
    }
}

// Made with Bob
