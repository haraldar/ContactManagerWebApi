using ContactManagerWebApi.Models;

namespace ContactManagerWebApi.Data;

/// <summary>
/// Simple interface to enforce a structure on the ContactsRepository.
/// </summary>
public interface IContactsRepository
{     
    Task<List<Contact>> GetContactsAsync();

    Task<Contact> GetContactAsync(int id);
    
    Task<Contact> InsertContactAsync(Contact contact);

    Task<bool> UpdateContactAsync(int id, Contact contact);
    
    Task<bool> DeleteContactAsync(int id);
}