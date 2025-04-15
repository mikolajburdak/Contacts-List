using System.Security.Claims;
using ContactsApp.Api.DTO;
using ContactsApp.Api.Models;
using ContactsApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;

namespace ContactsApp.Api.Controllers;

[ApiController] 
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private const string NameIdentifierClaimUri = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

    public ContactController(IContactService contactService)
    {
        _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ContactDto>> CreateContact([FromBody] CreateContactDto dto)
    {
        var userId = FindUserId();
        if (userId == null) 
        {
            return Unauthorized();
        }

        try
        {
            var contact = await _contactService.CreateContactAsync(dto, userId);
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the contact.", error = ex.Message });
        }
    }
    
    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdateContact(int id, [FromBody] ContactUpdateRequestDto dto)
    {
        var userId = FindUserId();
        if (userId == null) 
        {
            return Unauthorized();
        }

        var success = await _contactService.PartialUpdateContactAsync(id, dto, userId);
        return success ? NoContent() : NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactListDto>>> GetContacts()
    {
        var contacts = await _contactService.GetAllContactsAsync();
        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContact(int id)
    {
        try
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null)
                return NotFound();

            return Ok(contact);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the contact." });
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var userId = FindUserId();
        if (userId == null) 
        {
            return Unauthorized();
        }
        
        var contact = await _contactService.GetContactByIdAsync(id);
        if (contact == null)
            return NotFound();
            
        if (contact.UserId != userId)
            return Forbid();
        
        await _contactService.DeleteContactAsync(id);
        return NoContent();
    }
    
    private string? FindUserId()
    {
        // Wypisz wszystkie claims do debugowania
        foreach (var claim in User.Claims)
        {
            // Console.WriteLine logging removed
        }
        
        // WAŻNE: jti to identyfikator tokenu, nie użytkownika
        // Znajdź claim NameIdentifier, który zawiera UUID (a nie email)
        var userIdClaims = User.Claims
            .Where(c => 
                (c.Type == ClaimTypes.NameIdentifier || 
                 c.Type == NameIdentifierClaimUri) &&
                !c.Value.Contains('@') && // Not an email
                c.Value.Contains('-')  // UUID format
            )
            .ToList();
            
        if (userIdClaims.Any())
        {
            var userId = userIdClaims.First().Value;
            return userId;
        }
        
        // If no appropriate NameIdentifier found, look for other claims that might contain ID
        var fallbackClaims = User.Claims
            .Where(c => 
                !c.Value.Contains('@') && 
                c.Value.Contains('-') &&
                c.Value.Length > 20 &&
                c.Type != "jti" && // Not token identifier
                (c.Type.EndsWith("id") || c.Type.Contains("userid"))
            )
            .ToList();
            
        if (fallbackClaims.Any())
        {
            var userId = fallbackClaims.First().Value;
            return userId;
        }
        
        // Final attempt - use standard method
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            userIdClaim = User.FindFirst(NameIdentifierClaimUri);
        }
        
        return userIdClaim?.Value;
    }
}