using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ICustomerRepository _customerRepository;

    public CustomersController(ICustomerService customerService, ICustomerRepository customerRepository)
    {
        _customerService = customerService;
        _customerRepository = customerRepository;
    }

    /// <summary>მომხმარებლის სესხების ისტორია</summary>
    [HttpGet("loans")]
    [ProducesResponseType(typeof(IEnumerable<LoanResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerLoans([FromQuery] int customerId)
    {
        var result = await _customerService.GetCustomerLoansAsync(customerId);
        return Ok(result);
    }

    /// <summary>მომხმარებლის შექმნა</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CustomerResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto dto)
    {
        var existing = await _customerRepository.GetByPersonalNumberAsync(dto.PersonalNumber);
        if (existing != null)
            return BadRequest(new { error = "ეს პირადი ნომერი უკვე რეგისტრირებულია." });

        var customer = new Domain.Entities.Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PersonalNumber = dto.PersonalNumber,
            BirthDate = dto.BirthDate,
            CreditScore = dto.CreditScore
        };

        var created = await _customerRepository.AddAsync(customer);

        return StatusCode(StatusCodes.Status201Created, new CustomerResponseDto(
            created.Id, created.FirstName, created.LastName,
            created.PersonalNumber, created.BirthDate, created.CreditScore));
    }
}