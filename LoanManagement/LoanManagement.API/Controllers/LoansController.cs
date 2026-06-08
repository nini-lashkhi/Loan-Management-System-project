using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    /// <summary>სესხის განაცხადის შექმნა</summary>
    [HttpPost("CreateApplication")]
    [ProducesResponseType(typeof(LoanResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateApplication([FromBody] CreateLoanApplicationDto dto)
    {
        var result = await _loanService.CreateApplicationAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>სესხის სტატუსის მიღება</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LoanResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _loanService.GetByIdAsync(id);
        return Ok(result);
    }
}