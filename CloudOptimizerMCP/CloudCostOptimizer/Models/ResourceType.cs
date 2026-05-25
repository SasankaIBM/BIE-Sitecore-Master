namespace CloudCostOptimizer.Models
{
    public enum ResourceType
    {
        VirtualMachine,
        Database,
        Storage,
        LoadBalancer,
        Network,
        Container,
        Function,
        Cache,
        Queue,
        Other
    }

    public enum ResourceState
    {
        Running,
        Stopped,
        Idle,
        Oversized,
        Unattached,
        Zombie
    }

    public enum WasteCategory
    {
        IdleResources,
        OversizedInstances,
        UnattachedVolumes,
        StoppedResources,
        ZombieResources,
        RedundantBackups,
        UnusedReservations
    }

    public enum RecommendationPriority
    {
        High,
        Medium,
        Low
    }
}

// Made with Bob
