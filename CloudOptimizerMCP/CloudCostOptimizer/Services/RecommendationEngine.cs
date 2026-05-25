using CloudCostOptimizer.Models;

namespace CloudCostOptimizer.Services
{
    public class RecommendationEngine
    {
        private readonly WasteAnalyzer _wasteAnalyzer;

        public RecommendationEngine(WasteAnalyzer wasteAnalyzer)
        {
            _wasteAnalyzer = wasteAnalyzer;
        }

        public List<Recommendation> GenerateRecommendations(List<CloudResource> resources)
        {
            var recommendations = new List<Recommendation>();

            // Idle Resources Recommendations
            recommendations.AddRange(GenerateIdleResourceRecommendations(resources));

            // Oversized Instances Recommendations
            recommendations.AddRange(GenerateOversizedRecommendations(resources));

            // Unattached Volumes Recommendations
            recommendations.AddRange(GenerateUnattachedVolumeRecommendations(resources));

            // Stopped Resources Recommendations
            recommendations.AddRange(GenerateStoppedResourceRecommendations(resources));

            // Zombie Resources Recommendations
            recommendations.AddRange(GenerateZombieResourceRecommendations(resources));

            // Reserved Instances Recommendations
            recommendations.AddRange(GenerateReservedInstanceRecommendations(resources));

            // Scheduling Recommendations
            recommendations.AddRange(GenerateSchedulingRecommendations(resources));

            return recommendations.OrderByDescending(r => r.PotentialMonthlySavings).ToList();
        }

        private List<Recommendation> GenerateIdleResourceRecommendations(List<CloudResource> resources)
        {
            var recommendations = new List<Recommendation>();
            var idleResources = _wasteAnalyzer.AnalyzeIdleResources(resources);

            if (idleResources.Any())
            {
                var totalSavings = idleResources.Sum(r => r.PotentialSavings);
                recommendations.Add(new Recommendation
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Terminate Idle Resources",
                    Description = $"Found {idleResources.Count} idle resources with CPU < 5% and Memory < 10% for 7+ days. Consider terminating these resources.",
                    Priority = totalSavings > 1000 ? RecommendationPriority.High : RecommendationPriority.Medium,
                    Category = WasteCategory.IdleResources,
                    PotentialMonthlySavings = totalSavings,
                    AnnualSavings = totalSavings * 12,
                    ImplementationEffort = 2,
                    ImpactPercentage = (double)(totalSavings / resources.Sum(r => r.MonthlyCost) * 100),
                    ActionRequired = "Review and terminate idle resources after confirming they are not needed",
                    AffectedResources = idleResources.Select(r => r.ResourceName).ToList(),
                    ResourceCount = idleResources.Count,
                    Provider = "All"
                });
            }

            return recommendations;
        }

        private List<Recommendation> GenerateOversizedRecommendations(List<CloudResource> resources)
        {
            var recommendations = new List<Recommendation>();
            var oversizedResources = _wasteAnalyzer.AnalyzeOversizedResources(resources);

            if (oversizedResources.Any())
            {
                var totalSavings = oversizedResources.Sum(r => r.PotentialSavings);
                recommendations.Add(new Recommendation
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Downsize Oversized Instances",
                    Description = $"Found {oversizedResources.Count} oversized instances with consistent low utilization. Downsizing can reduce costs significantly.",
                    Priority = totalSavings > 800 ? RecommendationPriority.High : RecommendationPriority.Medium,
                    Category = WasteCategory.OversizedInstances,
                    PotentialMonthlySavings = totalSavings,
                    AnnualSavings = totalSavings * 12,
                    ImplementationEffort = 3,
                    ImpactPercentage = (double)(totalSavings / resources.Sum(r => r.MonthlyCost) * 100),
                    ActionRequired = "Resize instances to recommended smaller sizes during maintenance window",
                    AffectedResources = oversizedResources.Select(r => $"{r.ResourceName} ({r.InstanceSize} → {r.RecommendedSize})").ToList(),
                    ResourceCount = oversizedResources.Count,
                    Provider = "All"
                });
            }

            return recommendations;
        }

        private List<Recommendation> GenerateUnattachedVolumeRecommendations(List<CloudResource> resources)
        {
            var recommendations = new List<Recommendation>();
            var unattachedResources = _wasteAnalyzer.AnalyzeUnattachedResources(resources);

            if (unattachedResources.Any())
            {
                var totalSavings = unattachedResources.Sum(r => r.PotentialSavings);
                recommendations.Add(new Recommendation
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Delete Unattached Storage Volumes",
                    Description = $"Found {unattachedResources.Count} unattached storage volumes. These volumes are not attached to any instance and can be safely deleted after backup.",
                    Priority = RecommendationPriority.High,
                    Category = WasteCategory.UnattachedVolumes,
                    PotentialMonthlySavings = totalSavings,
                    AnnualSavings = totalSavings * 12,
                    ImplementationEffort = 1,
                    ImpactPercentage = (double)(totalSavings / resources.Sum(r => r.MonthlyCost) * 100),
                    ActionRequired = "Create snapshots for backup, then delete unattached volumes",
                    AffectedResources = unattachedResources.Select(r => r.ResourceName).ToList(),
                    ResourceCount = unattachedResources.Count,
                    Provider = "All"
                });
            }

            return recommendations;
        }

        private List<Recommendation> GenerateStoppedResourceRecommendations(List<CloudResource> resources)
        {
            var recommendations = new List<Recommendation>();
            var stoppedResources = _wasteAnalyzer.AnalyzeStoppedResources(resources);

            if (stoppedResources.Any())
            {
                var totalSavings = stoppedResources.Sum(r => r.PotentialSavings);
                recommendations.Add(new Recommendation
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Review Stopped Resources",
                    Description = $"Found {stoppedResources.Count} stopped resources still incurring costs. Consider terminating if no longer needed.",
                    Priority = RecommendationPriority.Medium,
                    Category = WasteCategory.StoppedResources,
                    PotentialMonthlySavings = totalSavings,
                    AnnualSavings = totalSavings * 12,
                    ImplementationEffort = 2,
                    ImpactPercentage = (double)(totalSavings / resources.Sum(r => r.MonthlyCost) * 100),
                    ActionRequired = "Review stopped resources and terminate those no longer needed",
                    AffectedResources = stoppedResources.Select(r => r.ResourceName).ToList(),
                    ResourceCount = stoppedResources.Count,
                    Provider = "All"
                });
            }

            return recommendations;
        }

        private List<Recommendation> GenerateZombieResourceRecommendations(List<CloudResource> resources)
        {
            var recommendations = new List<Recommendation>();
            var zombieResources = _wasteAnalyzer.AnalyzeZombieResources(resources);

            if (zombieResources.Any())
            {
                var totalSavings = zombieResources.Sum(r => r.PotentialSavings);
                recommendations.Add(new Recommendation
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Clean Up Zombie Resources",
                    Description = $"Found {zombieResources.Count} zombie resources with no activity for 30+ days. These are likely forgotten resources.",
                    Priority = RecommendationPriority.High,
                    Category = WasteCategory.ZombieResources,
                    PotentialMonthlySavings = totalSavings,
                    AnnualSavings = totalSavings * 12,
                    ImplementationEffort = 2,
                    ImpactPercentage = (double)(totalSavings / resources.Sum(r => r.MonthlyCost) * 100),
                    ActionRequired = "Identify owners and terminate zombie resources after confirmation",
                    AffectedResources = zombieResources.Select(r => r.ResourceName).ToList(),
                    ResourceCount = zombieResources.Count,
                    Provider = "All"
                });
            }

            return recommendations;
        }

        private List<Recommendation> GenerateReservedInstanceRecommendations(List<CloudResource> resources)
        {
            var recommendations = new List<Recommendation>();
            var stableWorkloads = resources.Where(r => 
                r.State == ResourceState.Running && 
                r.IsProduction && 
                (DateTime.UtcNow - r.CreatedDate).TotalDays > 90
            ).ToList();

            if (stableWorkloads.Any())
            {
                var potentialSavings = stableWorkloads.Sum(r => r.MonthlyCost) * 0.3m; // 30% savings with reserved instances
                recommendations.Add(new Recommendation
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Purchase Reserved Instances",
                    Description = $"Found {stableWorkloads.Count} stable production workloads running for 90+ days. Reserved instances can save up to 30-40%.",
                    Priority = potentialSavings > 1000 ? RecommendationPriority.High : RecommendationPriority.Medium,
                    Category = WasteCategory.UnusedReservations,
                    PotentialMonthlySavings = potentialSavings,
                    AnnualSavings = potentialSavings * 12,
                    ImplementationEffort = 3,
                    ImpactPercentage = (double)(potentialSavings / resources.Sum(r => r.MonthlyCost) * 100),
                    ActionRequired = "Analyze usage patterns and purchase 1-year or 3-year reserved instances",
                    AffectedResources = stableWorkloads.Select(r => r.ResourceName).Take(10).ToList(),
                    ResourceCount = stableWorkloads.Count,
                    Provider = "All"
                });
            }

            return recommendations;
        }

        private List<Recommendation> GenerateSchedulingRecommendations(List<CloudResource> resources)
        {
            var recommendations = new List<Recommendation>();
            var nonProdResources = resources.Where(r => 
                !r.IsProduction && 
                r.State == ResourceState.Running &&
                (r.Type == ResourceType.VirtualMachine || r.Type == ResourceType.Database)
            ).ToList();

            if (nonProdResources.Any())
            {
                var potentialSavings = nonProdResources.Sum(r => r.MonthlyCost) * 0.5m; // 50% savings with scheduling
                recommendations.Add(new Recommendation
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Implement Resource Scheduling",
                    Description = $"Found {nonProdResources.Count} non-production resources running 24/7. Schedule them to run only during business hours.",
                    Priority = potentialSavings > 500 ? RecommendationPriority.High : RecommendationPriority.Medium,
                    Category = WasteCategory.IdleResources,
                    PotentialMonthlySavings = potentialSavings,
                    AnnualSavings = potentialSavings * 12,
                    ImplementationEffort = 3,
                    ImpactPercentage = (double)(potentialSavings / resources.Sum(r => r.MonthlyCost) * 100),
                    ActionRequired = "Set up automated start/stop schedules for non-production resources",
                    AffectedResources = nonProdResources.Select(r => r.ResourceName).Take(10).ToList(),
                    ResourceCount = nonProdResources.Count,
                    Provider = "All"
                });
            }

            return recommendations;
        }

        public List<Recommendation> GetHighPriorityRecommendations(List<Recommendation> recommendations)
        {
            return recommendations.Where(r => r.Priority == RecommendationPriority.High).ToList();
        }

        public decimal CalculateTotalPotentialSavings(List<Recommendation> recommendations)
        {
            return recommendations.Sum(r => r.PotentialMonthlySavings);
        }
    }
}

// Made with Bob
