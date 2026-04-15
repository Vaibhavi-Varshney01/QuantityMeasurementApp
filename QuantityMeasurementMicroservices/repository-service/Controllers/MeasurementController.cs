using Microsoft.AspNetCore.Mvc;
using RepositoryService.Interface;
using SharedModels.Entities;

namespace RepositoryService.Controllers;

/// <summary>Internal API — consumed only by qma-service.</summary>
[ApiController]
[Route("internal/measurements")]
public class MeasurementController : ControllerBase
{
    private readonly IQMARepository _repo;
    public MeasurementController(IQMARepository repo) => _repo = repo;

    [HttpPost]
    public IActionResult Save([FromBody] QuantityMeasurementEntity entity)
    { _repo.Save(entity); return StatusCode(201); }

    [HttpGet("all")]
    public IActionResult GetAll() => Ok(_repo.GetAll());

    [HttpGet("operation/{operation}")]
    public IActionResult GetByOp(string operation) => Ok(_repo.GetByOperation(operation));

    [HttpGet("type/{type}")]
    public IActionResult GetByType(string type) => Ok(_repo.GetByType(type));

    [HttpGet("count/{operation}")]
    public IActionResult Count(string operation) => Ok(_repo.GetByOperation(operation).Count);

    [HttpGet("errors")]
    public IActionResult GetErrors() => Ok(_repo.GetAll().Where(e => e.HasError));

    [HttpDelete]
    public IActionResult DeleteAll() { _repo.DeleteAll(); return NoContent(); }
}
