using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideShareConnect.Data;
using RideShareConnect.Models.PayModel;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly AppDbContext _context;

    public WalletController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("balance")]
    public async Task<ActionResult<decimal>> GetBalance()
    {
        int userId = GetUserIdFromToken();
        if (userId == -1)
            return Unauthorized("UserId not found in token.");

        var wallet = await _context.Wallets
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wallet == null) return Ok(0);
        return Ok(wallet.Balance);
    }

    [HttpPost("add-money")]
    public async Task<IActionResult> AddMoney([FromBody] AddMoneyRequest request)
    {
        int userId = GetUserIdFromToken();
        if (userId == -1)
            return Unauthorized("UserId not found in token.");

        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        if (wallet == null)
        {
            wallet = new Wallet
            {
                UserId = userId,
                Balance = request.Amount
            };
            _context.Wallets.Add(wallet);
        }
        else
        {
            wallet.Balance += request.Amount;
            _context.Wallets.Update(wallet);
        }

        await _context.SaveChangesAsync();
        return Ok(new { wallet.Balance });
    }

    // ✅ Safe extraction with type checking
    private int GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return -1;

        return int.TryParse(userIdClaim, out int userId) ? userId : -1;
    }
}

public class AddMoneyRequest
{
    public decimal Amount { get; set; }
}