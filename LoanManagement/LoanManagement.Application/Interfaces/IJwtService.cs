using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace LoanManagement.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(string username);
}