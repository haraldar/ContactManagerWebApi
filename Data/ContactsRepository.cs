using Microsoft.EntityFrameworkCore;
using ContactManagerWebApi.Models;

namespace ContactManagerWebApi.Data;

/// <summary>
/// The implementation of the CRUD operations that are possible with the Contacts data.
/// </summary>
public class ContactsRepository : IContactsRepository
{

  private readonly ContactsDbContext _context;
  private readonly ILogger _logger;

  public ContactsRepository(ContactsDbContext context, ILoggerFactory loggerFactory)
  {
    _context = context;
    _logger = loggerFactory.CreateLogger("ContactsRepository");
  }


  /// <summary>
  /// Retrieves the contacts from the database.
  /// </summary>
  /// <returns>The list of contacts.</returns>
  public async Task<List<Contact>> GetContactsAsync()
    => await _context.Contacts.ToListAsync();


  /// <summary>
  /// Retrieves a contact from the database by its id.
  /// </summary>
  /// <param name="id">The identifier of the resource.</param>
  /// <returns>The contact by id.</returns>
  public async Task<Contact> GetContactAsync(int id)
    => await _context.Contacts.SingleOrDefaultAsync(c => c.Id == id);


  /// <summary>
  /// Inserts a new contact into the database.
  /// </summary>
  /// <param name="contact">The contact information to insert.</param>
  /// <returns>The contact object including all the generated data.</returns>
  public async Task<Contact> InsertContactAsync(Contact contact)
  {
    // Set the contact id to default, then add it to the context.
    contact.Id = 0;
    _context.Add(contact);

    // Try to save the contact to the database.
    try
    {
      await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError($"Error in {nameof(InsertContactAsync)}: " + ex.Message);
    }

    return contact;
  }


  /// <summary>
  /// Updates a contact in the database by id and contact information.
  /// </summary>
  /// <param name="id">The id of the contact to update.</param>
  /// <param name="contact">The contact data to apply to the existing object.</param>
  /// <returns>An indicator if it worked.</returns>
  public async Task<bool> UpdateContactAsync(int id, Contact contact)
  {
    // Get the contact to update by Id then replace the contents.
    // The reason for this is, that the updating of the row somehow resets the CreatedTimestamp.
    // We can avoid this, by chaniging the state of the properties and not the whole object.
    var contactToUpdate = _context.Contacts.Single(c => c.Id == id);
    contactToUpdate.FirstName = contact.FirstName;
    contactToUpdate.LastName = contact.LastName;
    contactToUpdate.DisplayName = contact.DisplayName;
    contactToUpdate.BirthDate = contact.BirthDate;
    contactToUpdate.Email = contact.Email;
    contactToUpdate.PhoneNumber = contact.PhoneNumber;

    // Try to apply the changes to the database.
    try
    {
      return (await _context.SaveChangesAsync() > 0 ? true : false);
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error in {nameof(UpdateContactAsync)}: " + ex.Message);
    }
    return false;
  }


  /// <summary>
  /// Delete a contact from the database.
  /// </summary>
  /// <param name="id">The id of the contact to delete.</param>
  /// <returns>An indicator if it worked.</returns>
  public async Task<bool> DeleteContactAsync(int id)
  {
    // Get the contact object from the database context and remove it.
    var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.Id == id);
    _context.Remove(contact);

    // Try to apply the changes to the database.
    try
    {
      return (await _context.SaveChangesAsync() > 0 ? true : false);
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error in {nameof(DeleteContactAsync)}: " + ex.Message);
    }
    return false;
  }

}