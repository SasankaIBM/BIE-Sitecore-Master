using CloudCostOptimizer.Models;

namespace CloudCostOptimizer.Services
{
    public class WasteAnalyzer
    {
        public List<CloudResource> AnalyzeIdleResources(List<CloudResource> resources)
        {
            return resources.Where(r =>
                r.State == ResourceState.Idle ||
                (r.CpuUsage < 5 && r.MemoryUsage < 10 && r.DaysIdle >= 7)
            ).ToList();
        }

        public List<CloudResource> AnalyzeOversizedResources(List<CloudResource> resources)
        {
            return resources.Where(r =>
                r.State == ResourceState.Oversized ||
                (r.CpuUsage < 20 && r.MemoryUsage < 30 && r.Type == ResourceType.VirtualMachine)
            ).ToList();
        }

        public List<CloudResource> AnalyzeUnattachedResources(List<CloudResource> resources)
        {
            return resources.Where(r =>
                r.State == ResourceState.Unattached ||
                (r.Type == ResourceType.Storage && r.DaysIdle > 30)
            ).ToList();
        }

        public List<CloudResource> AnalyzeStoppedResources(List<CloudResource> resources)
        {
            return resources.Where(r =>
                r.State == ResourceState.Stopped
            ).ToList();
        }

        public List<CloudResource> AnalyzeZombieResources(List<CloudResource> resources)
        {
            return resources.Where(r =>
                r.State == ResourceState.Zombie ||
                (DateTime.UtcNow - r.LastAccessDate).TotalDays > 30
            ).ToList();
        }

        public List<WasteAnalysis> GenerateWasteBreakdown(List<CloudResource> resources)
        {
            var breakdown = new List<WasteAnalysis>();
            var totalWaste = resources.Sum(r => r.EstimatedWaste);

            // Idle Resources
            var idleResources = AnalyzeIdleResources(resources);
            var idleWaste = idleResources.Sum(r => r.EstimatedWaste);
            breakdown.Add(new WasteAnalysis
            {
                Category = WasteCategory.IdleResources,
                CategoryName = "Idle Resources",
                TotalWaste = idleWaste,
                ResourceCount = idleResources.Count,
                Percentage = totalWaste > 0 ? (double)(idleWaste / totalWaste * 100) : 0,
                Description = "Resources with CPU < 5% and Memory < 10% for 7+ days"
            });

            // Oversized Instances
            var oversizedResources = AnalyzeOversizedResources(resources);
            var oversizedWaste = oversizedResources.Sum(r => r.EstimatedWaste);
            breakdown.Add(new WasteAnalysis
            {
                Category = WasteCategory.OversizedInstances,
                CategoryName = "Oversized Instances",
                TotalWaste = oversizedWaste,
                ResourceCount = oversizedResources.Count,
                Percentage = totalWaste > 0 ? (double)(oversizedWaste / totalWaste * 100) : 0,
                Description = "Instances that can be downsized based on usage patterns"
            });

            // Unattached Volumes
            var unattachedResources = AnalyzeUnattachedResources(resources);
            var unattachedWaste = unattachedResources.Sum(r => r.EstimatedWaste);
            breakdown.Add(new WasteAnalysis
            {
                Category = WasteCategory.UnattachedVolumes,
                CategoryName = "Unattached Volumes",
                TotalWaste = unattachedWaste,
                ResourceCount = unattachedResources.Count,
                Percentage = totalWaste > 0 ? (double)(unattachedWaste / totalWaste * 100) : 0,
                Description = "Storage volumes not attached to any instance"
            });

            // Stopped Resources
            var stoppedResources = AnalyzeStoppedResources(resources);
            var stoppedWaste = stoppedResources.Sum(r => r.EstimatedWaste);
            breakdown.Add(new WasteAnalysis
            {
                Category = WasteCategory.StoppedResources,
                CategoryName = "Stopped Resources",
                TotalWaste = stoppedWaste,
                ResourceCount = stoppedResources.Count,
                Percentage = totalWaste > 0 ? (double)(stoppedWaste / totalWaste * 100) : 0,
                Description = "Resources that are stopped but still incurring costs"
            });

            // Zombie Resources
            var zombieResources = AnalyzeZombieResources(resources);
            var zombieWaste = zombieResources.Sum(r => r.EstimatedWaste);
            breakdown.Add(new WasteAnalysis
            {
                Category = WasteCategory.ZombieResources,
                CategoryName = "Zombie Resources",
                TotalWaste = zombieWaste,
                ResourceCount = zombieResources.Count,
                Percentage = totalWaste > 0 ? (double)(zombieWaste / totalWaste * 100) : 0,
                Description = "Resources with no activity for 30+ days"
            });

            return breakdown.OrderByDescending(w => w.TotalWaste).ToList();
        }

        public CostSummary GenerateCostSummary(List<CloudResource> resources)
        {
            var summary = new CostSummary
            {
                TotalMonthlyCost = resources.Sum(r => r.MonthlyCost),
                TotalWaste = resources.Sum(r => r.EstimatedWaste),
                PotentialMonthlySavings = resources.Sum(r => r.PotentialSavings),
                TotalResources = resources.Count,
                IdleResourceCount = AnalyzeIdleResources(resources).Count,
                WasteBreakdown = GenerateWasteBreakdown(resources)
            };

            summary.PotentialAnnualSavings = summary.PotentialMonthlySavings * 12;
            summary.SavingsPercentage = summary.TotalMonthlyCost > 0 
                ? (double)(summary.PotentialMonthlySavings / summary.TotalMonthlyCost * 100) 
                : 0;

            // Cost by provider
            summary.CostByProvider = resources
                .GroupBy(r => r.Provider)
                .ToDictionary(g => g.Key, g => g.Sum(r => r.MonthlyCost));

            // Resources by type
            summary.ResourcesByType = resources
                .GroupBy(r => r.Type.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            return summary;
        }

        public Dictionary<string, decimal> CalculateWasteByProvider(List<CloudResource> resources)
        {
            return resources
                .GroupBy(r => r.Provider)
                .ToDictionary(g => g.Key, g => g.Sum(r => r.EstimatedWaste));
        }

        public List<CloudResource> GetTopWastefulResources(List<CloudResource> resources, int count = 10)
        {
            return resources
                .OrderByDescending(r => r.EstimatedWaste)
                .Take(count)
                .ToList();
        }
    }
}

// Made with Bob
