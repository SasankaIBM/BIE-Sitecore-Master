using CloudCostOptimizer.Models;

namespace CloudCostOptimizer.Services
{
    public class DataGenerator
    {
        private readonly Random _random = new Random();
        private readonly List<string> _awsRegions = new List<string> { "us-east-1", "us-west-2", "eu-west-1", "ap-southeast-1" };
        private readonly List<string> _azureRegions = new List<string> { "eastus", "westus2", "westeurope", "southeastasia" };
        private readonly List<string> _gcpRegions = new List<string> { "us-central1", "us-west1", "europe-west1", "asia-southeast1" };

        public List<CloudResource> GenerateResources()
        {
            var resources = new List<CloudResource>();

            // Generate AWS resources
            resources.AddRange(GenerateAWSResources(20));
            
            // Generate Azure resources
            resources.AddRange(GenerateAzureResources(18));
            
            // Generate GCP resources
            resources.AddRange(GenerateGCPResources(15));

            return resources;
        }

        private List<CloudResource> GenerateAWSResources(int count)
        {
            var resources = new List<CloudResource>();
            var instanceTypes = new[] { "t3.micro", "t3.small", "t3.medium", "t3.large", "m5.large", "m5.xlarge", "c5.large" };
            var resourceTypes = new[] { ResourceType.VirtualMachine, ResourceType.Database, ResourceType.Storage, ResourceType.LoadBalancer };

            for (int i = 0; i < count; i++)
            {
                var type = resourceTypes[_random.Next(resourceTypes.Length)];
                var state = GetRandomState();
                var instanceSize = instanceTypes[_random.Next(instanceTypes.Length)];
                var monthlyCost = GetCostByInstanceType(instanceSize);
                var cpuUsage = state == ResourceState.Idle ? _random.Next(1, 5) : _random.Next(5, 95);
                var memoryUsage = state == ResourceState.Idle ? _random.Next(5, 15) : _random.Next(15, 90);

                resources.Add(new CloudResource
                {
                    ResourceId = $"aws-{Guid.NewGuid().ToString().Substring(0, 8)}",
                    ResourceName = $"aws-{type.ToString().ToLower()}-{(i + 1):D3}",
                    Provider = "AWS",
                    Type = type,
                    State = state,
                    Region = _awsRegions[_random.Next(_awsRegions.Count)],
                    MonthlyCost = monthlyCost,
                    CpuUsage = cpuUsage,
                    MemoryUsage = memoryUsage,
                    StorageUsage = _random.Next(10, 90),
                    DaysIdle = state == ResourceState.Idle ? _random.Next(7, 45) : 0,
                    CreatedDate = DateTime.UtcNow.AddDays(-_random.Next(30, 365)),
                    LastAccessDate = DateTime.UtcNow.AddDays(-_random.Next(0, 30)),
                    InstanceSize = instanceSize,
                    RecommendedSize = state == ResourceState.Oversized ? GetSmallerInstanceType(instanceSize) : instanceSize,
                    EstimatedWaste = CalculateWaste(monthlyCost, state, cpuUsage),
                    PotentialSavings = CalculateSavings(monthlyCost, state, instanceSize),
                    Tags = GenerateTags(),
                    IsProduction = _random.Next(0, 100) > 30
                });
            }

            return resources;
        }

        private List<CloudResource> GenerateAzureResources(int count)
        {
            var resources = new List<CloudResource>();
            var instanceTypes = new[] { "B1s", "B2s", "D2s_v3", "D4s_v3", "E2s_v3", "F2s_v2" };
            var resourceTypes = new[] { ResourceType.VirtualMachine, ResourceType.Database, ResourceType.Storage, ResourceType.Container };

            for (int i = 0; i < count; i++)
            {
                var type = resourceTypes[_random.Next(resourceTypes.Length)];
                var state = GetRandomState();
                var instanceSize = instanceTypes[_random.Next(instanceTypes.Length)];
                var monthlyCost = GetCostByInstanceType(instanceSize);
                var cpuUsage = state == ResourceState.Idle ? _random.Next(1, 5) : _random.Next(5, 95);
                var memoryUsage = state == ResourceState.Idle ? _random.Next(5, 15) : _random.Next(15, 90);

                resources.Add(new CloudResource
                {
                    ResourceId = $"azure-{Guid.NewGuid().ToString().Substring(0, 8)}",
                    ResourceName = $"azure-{type.ToString().ToLower()}-{(i + 1):D3}",
                    Provider = "Azure",
                    Type = type,
                    State = state,
                    Region = _azureRegions[_random.Next(_azureRegions.Count)],
                    MonthlyCost = monthlyCost,
                    CpuUsage = cpuUsage,
                    MemoryUsage = memoryUsage,
                    StorageUsage = _random.Next(10, 90),
                    DaysIdle = state == ResourceState.Idle ? _random.Next(7, 45) : 0,
                    CreatedDate = DateTime.UtcNow.AddDays(-_random.Next(30, 365)),
                    LastAccessDate = DateTime.UtcNow.AddDays(-_random.Next(0, 30)),
                    InstanceSize = instanceSize,
                    RecommendedSize = state == ResourceState.Oversized ? GetSmallerInstanceType(instanceSize) : instanceSize,
                    EstimatedWaste = CalculateWaste(monthlyCost, state, cpuUsage),
                    PotentialSavings = CalculateSavings(monthlyCost, state, instanceSize),
                    Tags = GenerateTags(),
                    IsProduction = _random.Next(0, 100) > 30
                });
            }

            return resources;
        }

        private List<CloudResource> GenerateGCPResources(int count)
        {
            var resources = new List<CloudResource>();
            var instanceTypes = new[] { "e2-micro", "e2-small", "e2-medium", "n1-standard-1", "n1-standard-2", "n2-standard-2" };
            var resourceTypes = new[] { ResourceType.VirtualMachine, ResourceType.Database, ResourceType.Storage, ResourceType.Function };

            for (int i = 0; i < count; i++)
            {
                var type = resourceTypes[_random.Next(resourceTypes.Length)];
                var state = GetRandomState();
                var instanceSize = instanceTypes[_random.Next(instanceTypes.Length)];
                var monthlyCost = GetCostByInstanceType(instanceSize);
                var cpuUsage = state == ResourceState.Idle ? _random.Next(1, 5) : _random.Next(5, 95);
                var memoryUsage = state == ResourceState.Idle ? _random.Next(5, 15) : _random.Next(15, 90);

                resources.Add(new CloudResource
                {
                    ResourceId = $"gcp-{Guid.NewGuid().ToString().Substring(0, 8)}",
                    ResourceName = $"gcp-{type.ToString().ToLower()}-{(i + 1):D3}",
                    Provider = "GCP",
                    Type = type,
                    State = state,
                    Region = _gcpRegions[_random.Next(_gcpRegions.Count)],
                    MonthlyCost = monthlyCost,
                    CpuUsage = cpuUsage,
                    MemoryUsage = memoryUsage,
                    StorageUsage = _random.Next(10, 90),
                    DaysIdle = state == ResourceState.Idle ? _random.Next(7, 45) : 0,
                    CreatedDate = DateTime.UtcNow.AddDays(-_random.Next(30, 365)),
                    LastAccessDate = DateTime.UtcNow.AddDays(-_random.Next(0, 30)),
                    InstanceSize = instanceSize,
                    RecommendedSize = state == ResourceState.Oversized ? GetSmallerInstanceType(instanceSize) : instanceSize,
                    EstimatedWaste = CalculateWaste(monthlyCost, state, cpuUsage),
                    PotentialSavings = CalculateSavings(monthlyCost, state, instanceSize),
                    Tags = GenerateTags(),
                    IsProduction = _random.Next(0, 100) > 30
                });
            }

            return resources;
        }

        private ResourceState GetRandomState()
        {
            var rand = _random.Next(0, 100);
            if (rand < 20) return ResourceState.Idle;
            if (rand < 35) return ResourceState.Oversized;
            if (rand < 45) return ResourceState.Stopped;
            if (rand < 55) return ResourceState.Unattached;
            if (rand < 65) return ResourceState.Zombie;
            return ResourceState.Running;
        }

        private decimal GetCostByInstanceType(string instanceType)
        {
            var costs = new Dictionary<string, decimal>
            {
                { "t3.micro", 7.5m }, { "t3.small", 15m }, { "t3.medium", 30m }, { "t3.large", 60m },
                { "m5.large", 70m }, { "m5.xlarge", 140m }, { "c5.large", 65m },
                { "B1s", 8m }, { "B2s", 30m }, { "D2s_v3", 70m }, { "D4s_v3", 140m },
                { "E2s_v3", 120m }, { "F2s_v2", 75m },
                { "e2-micro", 6m }, { "e2-small", 13m }, { "e2-medium", 27m },
                { "n1-standard-1", 25m }, { "n1-standard-2", 50m }, { "n2-standard-2", 65m }
            };

            return costs.ContainsKey(instanceType) ? costs[instanceType] : 50m;
        }

        private string GetSmallerInstanceType(string currentType)
        {
            var downsizeMap = new Dictionary<string, string>
            {
                { "t3.large", "t3.medium" }, { "t3.medium", "t3.small" }, { "m5.xlarge", "m5.large" },
                { "D4s_v3", "D2s_v3" }, { "E2s_v3", "B2s" },
                { "n1-standard-2", "n1-standard-1" }, { "n2-standard-2", "e2-medium" }
            };

            return downsizeMap.ContainsKey(currentType) ? downsizeMap[currentType] : currentType;
        }

        private decimal CalculateWaste(decimal monthlyCost, ResourceState state, double cpuUsage)
        {
            return state switch
            {
                ResourceState.Idle => monthlyCost * 0.9m,
                ResourceState.Stopped => monthlyCost * 0.3m,
                ResourceState.Oversized => monthlyCost * 0.4m,
                ResourceState.Unattached => monthlyCost * 1.0m,
                ResourceState.Zombie => monthlyCost * 0.95m,
                _ => cpuUsage < 10 ? monthlyCost * 0.5m : 0m
            };
        }

        private decimal CalculateSavings(decimal monthlyCost, ResourceState state, string instanceSize)
        {
            return state switch
            {
                ResourceState.Idle => monthlyCost * 0.85m,
                ResourceState.Stopped => monthlyCost * 0.25m,
                ResourceState.Oversized => monthlyCost * 0.35m,
                ResourceState.Unattached => monthlyCost,
                ResourceState.Zombie => monthlyCost * 0.9m,
                _ => 0m
            };
        }

        private List<string> GenerateTags()
        {
            var allTags = new[] { "production", "development", "testing", "staging", "critical", "non-critical", "backup", "temporary" };
            var tagCount = _random.Next(1, 4);
            return allTags.OrderBy(x => _random.Next()).Take(tagCount).ToList();
        }

        public List<CostTrend> GenerateCostTrends()
        {
            var trends = new List<CostTrend>();
            var providers = new[] { "AWS", "Azure", "GCP" };
            var baseDate = DateTime.UtcNow.AddMonths(-6);

            foreach (var provider in providers)
            {
                decimal baseCost = provider switch
                {
                    "AWS" => 12000m,
                    "Azure" => 9500m,
                    "GCP" => 7000m,
                    _ => 5000m
                };

                for (int i = 0; i < 180; i += 7) // Weekly data for 6 months
                {
                    var variance = (decimal)(_random.NextDouble() * 0.2 - 0.1); // ±10% variance
                    var trendFactor = 1 + (i / 1800m); // Slight upward trend
                    
                    trends.Add(new CostTrend
                    {
                        Date = baseDate.AddDays(i),
                        TotalCost = baseCost * trendFactor * (1 + variance),
                        Waste = baseCost * trendFactor * (1 + variance) * 0.25m,
                        Provider = provider
                    });
                }
            }

            return trends.OrderBy(t => t.Date).ToList();
        }
    }
}

// Made with Bob
