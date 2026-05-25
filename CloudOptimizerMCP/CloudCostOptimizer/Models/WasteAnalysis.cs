namespace CloudCostOptimizer.Models
{
    public class WasteAnalysis
    {
        public WasteCategory Category { get; set; }
        public string CategoryName { get; set; }
        public decimal TotalWaste { get; set; }
        public int ResourceCount { get; set; }
        public double Percentage { get; set; }
        public string Description { get; set; }
    }

    public class CostSummary
    {
        public decimal TotalMonthlyCost { get; set; }
        public decimal TotalWaste { get; set; }
        public decimal PotentialMonthlySavings { get; set; }
        public decimal PotentialAnnualSavings { get; set; }
        public double SavingsPercentage { get; set; }
        public int TotalResources { get; set; }
        public int IdleResourceCount { get; set; }
        public int HighPriorityRecommendations { get; set; }
        public List<WasteAnalysis> WasteBreakdown { get; set; }
        public Dictionary<string, decimal> CostByProvider { get; set; }
        public Dictionary<string, int> ResourcesByType { get; set; }

        public CostSummary()
        {
            WasteBreakdown = new List<WasteAnalysis>();
            CostByProvider = new Dictionary<string, decimal>();
            ResourcesByType = new Dictionary<string, int>();
        }
    }

    public class CostTrend
    {
        public DateTime Date { get; set; }
        public decimal TotalCost { get; set; }
        public decimal Waste { get; set; }
        public string Provider { get; set; }
    }
}

// Made with Bob
