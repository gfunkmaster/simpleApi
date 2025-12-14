namespace SimpleApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SimpleApi.src.Models;

/// <summary>
/// Hanterar användarkonto-operationer som aktivering och deaktivering
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(UserManager<ApplicationUser> userManager) : ControllerBase
{
    /// <summary>
    /// Deaktiverar den inloggade användarens konto (sätter IsActive = false)
    /// </summary>
    /// <returns>Bekräftelse på deaktivering</returns>
    /// <response code="200">Användaren deaktiverad</response>
    /// <response code="400">Fel vid uppdatering</response>
    /// <response code="401">Användaren är inte autentiserad</response>
    /// <response code="404">Användaren hittades inte</response>
    [HttpPost("deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeactivateCurrentUser()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type.EndsWith("nameidentifier"))?.Value;
        if (userId == null)
            return Unauthorized();

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound("User not found");

        user.IsActive = false;
        var result = await userManager.UpdateAsync(user);

        if (result.Succeeded)
            return Ok(new { message = "User deactivated successfully", email = user.Email });

        return BadRequest(result.Errors);
    }

    /// <summary>
    /// Aktiverar den inloggade användarens konto (sätter IsActive = true)
    /// </summary>
    /// <returns>Bekräftelse på aktivering</returns>
    /// <response code="200">Användaren aktiverad</response>
    /// <response code="400">Fel vid uppdatering</response>
    /// <response code="401">Användaren är inte autentiserad</response>
    /// <response code="404">Användaren hittades inte</response>
    [HttpPost("activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ActivateCurrentUser()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type.EndsWith("nameidentifier"))?.Value;
        if (userId == null)
            return Unauthorized();

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound("User not found");

        user.IsActive = true;
        var result = await userManager.UpdateAsync(user);

        if (result.Succeeded)
            return Ok(new { message = "User activated successfully", email = user.Email });

        return BadRequest(result.Errors);
    }

    /// <summary>
    /// Hämtar den inloggade användarens status och information
    /// </summary>
    /// <returns>Användarens ID, email och IsActive status</returns>
    /// <response code="200">Returnerar användarstatus</response>
    /// <response code="401">Användaren är inte autentiserad</response>
    /// <response code="404">Användaren hittades inte</response>
    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetUserStatus()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type.EndsWith("nameidentifier"))?.Value;
        if (userId == null)
            return Unauthorized();

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound("User not found");

        return Ok(new { 
            userId = user.Id,
            email = user.Email, 
            isActive = user.IsActive 
        });
    }
}
