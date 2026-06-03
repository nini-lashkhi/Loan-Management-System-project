using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>გადახდის განხორციელება</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PaymentResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MakePayment([FromBody] CreatePaymentDto dto)
    {
        var result = await _paymentService.MakePaymentAsync(dto);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}