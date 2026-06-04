using Application_Layer.Features.Files.Queries;
using Application_Layer.Interfaces;
using Infrastructure_Layer.Configuration;
using Infrastructure_Layer.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;



namespace API_Layer.Controllers; 

/// <summary>
/// Controller för att hantera filflöde och övervakning.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFlowMonitoringService _flowMonitoringService;

    /// <summary>
    /// Constructor med dependency injection.
    /// Hämtar in FlowMonitoringService från DI-container.
    /// </summary>
    public FilesController(
        IFlowMonitoringService flowMonitoringService)
    {
        _flowMonitoringService = flowMonitoringService;
    }
     
    /// <summary>
    /// Hämtar status för hela filflödet.
    /// </summary>
    [HttpGet("Fileflow-status")]
    public IActionResult GetFlowStatus()
    {
        var result = _flowMonitoringService.GetFlowStatus();
        return Ok(result);
    }
}

