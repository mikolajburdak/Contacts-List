using ContactsApp.Api.DTO;
using ContactsApp.Api.Models;

namespace ContactsApp.Api.Mappers;

public static class ContactMapper
{
    public static void MapUpdate(Contact contact, ContactUpdateRequestDto contactUpdateRequestDto)
    {
        if (contactUpdateRequestDto.FirstName is not null) contact.FirstName = contactUpdateRequestDto.FirstName;
        if (contactUpdateRequestDto.LastName is not null) contact.LastName = contactUpdateRequestDto.LastName;
        if (contactUpdateRequestDto.Email is not null) contact.Email = contactUpdateRequestDto.Email;
        if (contactUpdateRequestDto.PhoneNumber is not null) contact.PhoneNumber = contactUpdateRequestDto.PhoneNumber;
        if (contactUpdateRequestDto.BirthDate.HasValue) contact.BirthDate = contactUpdateRequestDto.BirthDate.Value;
        if (contactUpdateRequestDto.CategoryId.HasValue) contact.CategoryId = contactUpdateRequestDto.CategoryId.Value;
        if (contactUpdateRequestDto.SubcategoryId.HasValue) contact.SubcategoryId = contactUpdateRequestDto.SubcategoryId.Value;
    }
    
    public static Contact ToModel(ContactDto dto, string userId)
    {
        return new Contact
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            BirthDate = dto.BirthDate,
            CategoryId = dto.CategoryId,
            SubcategoryId = dto.SubcategoryId,
            UserId = userId // Przypisanie UserId
        };
    }

    public static Contact ToContact(CreateContactDto dto, string userId)
    {
        return new Contact
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            BirthDate = dto.BirthDate,
            CategoryId = dto.CategoryId,
            SubcategoryId = dto.SubcategoryId,
            UserId = userId
        };
    }

    public static ContactDto ToDto(Contact contact)
    {
        return new ContactDto
        {
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            PhoneNumber = contact.PhoneNumber,
            BirthDate = contact.BirthDate,
            CategoryId = contact.CategoryId,
            SubcategoryId = contact.SubcategoryId,
        };
    }

    public static ContactListDto ToContactListDto(CreateContactList createContactList)
    {
        return new ContactListDto
        {
            Id = createContactList.Id,
            FirstName = createContactList.FirstName,
            LastName = createContactList.LastName,
        };
    }
    
    public static IEnumerable<ContactListDto> ToDtoList(IEnumerable<CreateContactList> contacts)
    {
        return contacts.Select(ToContactListDto);
    }
}