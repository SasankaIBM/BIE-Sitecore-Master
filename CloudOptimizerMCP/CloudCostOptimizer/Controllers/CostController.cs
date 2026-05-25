using CloudCostOptimizer.Models;
using CloudCostOptimizer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloudCostOptimizer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CostController : ControllerBase
{
    private readonly CostService _costService;

    public CostController(CostService costService)
    {
        _costService = costService;
    }

    // Legacy endpoints - kept for backward compatibility
    [HttpGet("costs")]
    public IActionResult GetCosts()
    {
        return Ok(_costService.GetCosts());
    }

    [HttpGet("idle-resources")]
    public IActionResult GetIdleResources()
    {
        return Ok(_costService.GetIdleResources());
    }

    [HttpGet("recommendations")]
    public IActionResult GetRecommendations()
    {
        return Ok(_costService.GetRecommendations());
    }

    // New enhanced endpoints
    [HttpGet("summary")]
    public IActionResult GetCostSummary()
    {
        try
        {
            var summary = _costService.GetCostSummary();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("waste-breakdown")]
    public IActionResult GetWasteBreakdown()
    {
        try
        {
            var breakdown = _costService.GetWasteBreakdown();
            return Ok(breakdown);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("resources")]
    public IActionResult GetAllResources()
    {
        try
        {
            var resources = _costService.GetAllResources();
            return Ok(resources);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("resources/provider/{provider}")]
    public IActionResult GetResourcesByProvider(string provider)
    {
        try
        {
            var resources = _costService.GetResourcesByProvider(provider);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("resources/type/{type}")]
    public IActionResult GetResourcesByType(ResourceType type)
    {
        try
        {
            var resources = _costService.GetResourcesByType(type);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("resources/state/{state}")]
    public IActionResult GetResourcesByState(ResourceState state)
    {
        try
        {
            var resources = _costService.GetResourcesByState(state);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("recommendations/detailed")]
    public IActionResult GetDetailedRecommendations()
    {
        try
        {
            var recommendations = _costService.GetDetailedRecommendations();
            return Ok(recommendations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("recommendations/high-priority")]
    public IActionResult GetHighPriorityRecommendations()
    {
        try
        {
            var recommendations = _costService.GetHighPriorityRecommendations();
            return Ok(recommendations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("trends")]
    public IActionResult GetCostTrends()
    {
        try
        {
            var trends = _costService.GetCostTrends();
            return Ok(trends);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("trends/provider/{provider}")]
    public IActionResult GetCostTrendsByProvider(string provider)
    {
        try
        {
            var trends = _costService.GetCostTrendsByProvider(provider);
            return Ok(trends);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("top-wasteful")]
    public IActionResult GetTopWastefulResources([FromQuery] int count = 10)
    {
        try
        {
            var resources = _costService.GetTopWastefulResources(count);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("waste-by-provider")]
    public IActionResult GetWasteByProvider()
    {
        try
        {
            var waste = _costService.GetWasteByProvider();
            return Ok(waste);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("potential-savings")]
    public IActionResult GetTotalPotentialSavings()
    {
        try
        {
            var savings = _costService.GetTotalPotentialSavings();
            return Ok(new { totalPotentialMonthlySavings = savings, totalPotentialAnnualSavings = savings * 12 });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public IActionResult RefreshData()
    {
        try
        {
            _costService.RefreshData();
            return Ok(new { message = "Data refreshed successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}