using System.Security.Claims;
using ContactsApp.Api.DTO;
using ContactsApp.Api.Models;
using ContactsApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Api.Controllers;

[Authorize]
[ApiController] 
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }
    
    [HttpPost]
    public async Task<ActionResult<ContactDto>> CreateContact([FromBody] CreateContactDto dto)
    {
        // Właściwy format tokena to "Bearer {token}"
        var authHeader = Request.Headers["Authorization"].ToString();
        Console.WriteLine($"[DEBUG] Authorization Header: {authHeader}");
        
        // Sprawdzamy czy token jest w prawidłowym formacie
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            Console.WriteLine("[DEBUG] Invalid token format or missing token");
            return Unauthorized();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine($"[DEBUG] USER ID: {userId}");

        if (userId == null) return Unauthorized();

        var contact = await _contactService.CreateContactAsync(dto, userId);
        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdateContact(int id, [FromBody] ContactUpdateRequestDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var success = await _contactService.PartialUpdateContactAsync(id, dto, userId);
        return success ? NoContent() : NotFound();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactListDto>>> GetContacts()
    {
        var contactList = await _contactService.GetAllContactsAsync();
        return Ok(contactList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            Console.WriteLine("[DEBUG] GetContact: No userId found in claims");
            return Unauthorized();
        }
        
        var contact = await _contactService.GetContactByIdAsync(id);
        if (contact == null)
        {
            return NotFound();
        }
        return Ok(contact);
    }

    /*[HttpPost]
    public async Task<ActionResult<ContactDto>> CreateContact([FromBody]ContactDto contactDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }
        var contact = await _contactService.CreateContactAsync(contactDto, userId);
        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
    }*/

    /*[HttpPatch("{id}")]
    public async Task<ActionResult> PartialUpdateContact(int id, [FromBody]ContactUpdateRequestDto requestDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }
        var success = await _contactService.PartialUpdateContactAsync(id, requestDto, userId);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }*/

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteContact(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            Console.WriteLine("[DEBUG] DeleteContact: No userId found in claims");
            return Unauthorized();
        }
        
        await _contactService.DeleteContactAsync(id);
        return NoContent();
    }
    
}