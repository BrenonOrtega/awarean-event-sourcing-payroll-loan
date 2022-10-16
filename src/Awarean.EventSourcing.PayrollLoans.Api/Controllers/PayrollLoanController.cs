using System.ComponentModel.DataAnnotations;
using Awarean.EventSourcing.PayrollLoans.Api.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Awarean.EventSourcing.PayrollLoans.Api.Entities.PayrollLoans.Commands;

namespace Awarean.EventSourcing.PayrollLoans.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayrollLoanController : ControllerBase
{
    private readonly IPayrollLoanService _service;

    public PayrollLoanController(IPayrollLoanService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody][Required] CreateLoanCommand command)
    {
        var result = await _service.CreateLoan(command);

        if (result.IsFailed)
            return BadRequest(result.Error);

        return Accepted();
    }

    [HttpPut]
    public async Task<IActionResult> Pay([FromBody][Required] PayInstallmentCommand command)
    {
        var result = await _service.PayInstallments(command);

        if (result.IsFailed)
            return BadRequest(result.Error);

        return Ok();
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetLoan(id);

        if (result.IsFailed)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }
    
    #if DEBUG
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }
    #endif
}
