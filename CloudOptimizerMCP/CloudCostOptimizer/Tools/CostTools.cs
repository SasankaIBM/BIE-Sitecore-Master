using System.ComponentModel;
using CloudCostOptimizer.Services;
using ModelContextProtocol.Server;

namespace CloudCostOptimizer.Tools;

[McpServerToolType]
public class CostTools
{
    private readonly CostService _costService;

    public CostTools(CostService costService)
    {
        _costService = costService;
    }

    [McpServerTool, Description("Get all cloud costs")]
    public object GetCloudCosts()
    {
        return _costService.GetCosts();
    }

    [McpServerTool, Description("Detect idle cloud resources")]
    public object DetectIdleResources()
    {
        return _costService.GetIdleResources();
    }

    [McpServerTool, Description("Get optimization recommendations")]
    public object GetRecommendations()
    {
        return _costService.GetRecommendations();
    }
}