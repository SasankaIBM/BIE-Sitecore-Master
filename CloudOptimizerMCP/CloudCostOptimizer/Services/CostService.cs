using CloudCostOptimizer.Models;

namespace CloudCostOptimizer.Services;

public class CostService
{
    private readonly DataGenerator _dataGenerator;
    private readonly WasteAnalyzer _wasteAnalyzer;
    private readonly RecommendationEngine _recommendationEngine;
    private List<CloudResource> _cachedResources;
    private List<CostTrend> _cachedTrends;

    public CostService()
    {
        _dataGenerator = new DataGenerator();
        _wasteAnalyzer = new WasteAnalyzer();
        _recommendationEngine = new RecommendationEngine(_wasteAnalyzer);
        _cachedResources = _dataGenerator.GenerateResources();
        _cachedTrends = _dataGenerator.GenerateCostTrends();
    }

    // Legacy method - kept for backward compatibility
    public List<CloudCost> GetCosts()
    {
        var resources = _cachedResources;
        var costByProvider = resources.GroupBy(r => r.Provider)
            .Select(g => new CloudCost
            {
                Provider = g.Key,
                MonthlyCost = g.Sum(r => r.MonthlyCost),
                ForecastCost = g.Sum(r => r.MonthlyCost) * 1.15m // 15% forecast increase
            }).ToList();

        return costByProvider;
    }

    // Legacy method - kept for backward compatibility
    public List<IdleResource> GetIdleResources()
    {
        var idleResources = _wasteAnalyzer.AnalyzeIdleResources(_cachedResources);
        return idleResources.Select(r => new IdleResource
        {
            ResourceName = r.ResourceName,
            Type = r.Type.ToString(),
            EstimatedWaste = r.EstimatedWaste,
            CpuUsage = r.CpuUsage
        }).ToList();
    }

    // Legacy method - kept for backward compatibility
    public List<string> GetRecommendations()
    {
        var recommendations = _recommendationEngine.GenerateRecommendations(_cachedResources);
        return recommendations.Take(10).Select(r => r.Title).ToList();
    }

    // New enhanced methods
    public CostSummary GetCostSummary()
    {
        var summary = _wasteAnalyzer.GenerateCostSummary(_cachedResources);
        summary.HighPriorityRecommendations = _recommendationEngine
            .GenerateRecommendations(_cachedResources)
            .Count(r => r.Priority == RecommendationPriority.High);
        return summary;
    }

    public List<WasteAnalysis> GetWasteBreakdown()
    {
        return _wasteAnalyzer.GenerateWasteBreakdown(_cachedResources);
    }

    public List<CloudResource> GetAllResources()
    {
        return _cachedResources;
    }

    public List<CloudResource> GetResourcesByProvider(string provider)
    {
        return _cachedResources.Where(r => r.Provider.Equals(provider, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<CloudResource> GetResourcesByType(ResourceType type)
    {
        return _cachedResources.Where(r => r.Type == type).ToList();
    }

    public List<CloudResource> GetResourcesByState(ResourceState state)
    {
        return _cachedResources.Where(r => r.State == state).ToList();
    }

    public List<Recommendation> GetDetailedRecommendations()
    {
        return _recommendationEngine.GenerateRecommendations(_cachedResources);
    }

    public List<Recommendation> GetHighPriorityRecommendations()
    {
        var allRecommendations = _recommendationEngine.GenerateRecommendations(_cachedResources);
        return _recommendationEngine.GetHighPriorityRecommendations(allRecommendations);
    }

    public List<CostTrend> GetCostTrends()
    {
        return _cachedTrends;
    }

    public List<CostTrend> GetCostTrendsByProvider(string provider)
    {
        return _cachedTrends.Where(t => t.Provider.Equals(provider, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<CloudResource> GetTopWastefulResources(int count = 10)
    {
        return _wasteAnalyzer.GetTopWastefulResources(_cachedResources, count);
    }

    public Dictionary<string, decimal> GetWasteByProvider()
    {
        return _wasteAnalyzer.CalculateWasteByProvider(_cachedResources);
    }

    public decimal GetTotalPotentialSavings()
    {
        var recommendations = _recommendationEngine.GenerateRecommendations(_cachedResources);
        return _recommendationEngine.CalculateTotalPotentialSavings(recommendations);
    }

    // Refresh data (useful for testing)
    public void RefreshData()
    {
        _cachedResources = _dataGenerator.GenerateResources();
        _cachedTrends = _dataGenerator.GenerateCostTrends();
    }
}