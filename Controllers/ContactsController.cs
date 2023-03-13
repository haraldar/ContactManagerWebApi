using Microsoft.AspNetCore.Mvc;
using ContactManagerWebApi.Models;
using ContactManagerWebApi.Data;

namespace ContactManagerWebApi.Controllers;


[ApiController]
[Route("contacts")]
/// <summary>
/// Simple CRUD Controller class for setting up the endpoints and handling the data in the database.
/// </summary>
public class ContactsController : ControllerBase
{
    private IContactsRepository _repo;

    public ContactsController(IContactsRepository repo)
      => _repo = repo;
      

    // GET /contacts
    /// <summary>
    /// Retrieves the list of contacts from the database.
    /// </summary>
    /// <returns>The list of contacts.</returns>
    [HttpGet()]
    [ProducesResponseType(typeof(List<Contact>), 200)]
    [ProducesResponseType(typeof(List<Contact>), 404)]
    public async Task<ActionResult> GetContacts()
    {
        var contacts = await _repo.GetContactsAsync();
        return contacts == null
          ? NotFound()
          : Ok(contacts);
    }


    // GET /contacts/<int:id>
    /// <summary>
    /// Retrieves the contact specified by the id from the database.
    /// </summary>
    /// <param name="id">The primary key.</param>
    /// <returns>The contact.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Contact), 200)]
    [ProducesResponseType(typeof(Contact), 404)]
    public async Task<ActionResult> GetContact(int id)
    {
        var contact = await _repo.GetContactAsync(id);
        return contact == null
          ? NotFound()
          : Ok(contact);
    }


    // POST /contacts
    /// <summary>
    /// Creates a new contact in the database.
    /// </summary>
    /// <param name="contact">The contact information.</param>
    /// <returns>The new full contact.</returns>
    [HttpPost()]
    [ProducesResponseType(typeof(Contact), 201)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<ActionResult> PostContact([FromBody]Contact contact)
    {
      if (!ModelState.IsValid)
        return BadRequest(this.ModelState);

      var newContact = await _repo.InsertContactAsync(contact);
      if (newContact == null)
        return BadRequest("Could not insert new contact into the database.");

      return Created(nameof(PostContact), newContact);
    }


    // PUT /contacts/<int:id>
    /// <summary>
    /// Updates a contact's information in the database. 
    /// </summary>
    /// <param name="id">The id of the contact to update.</param>
    /// <param name="contact">The contact information that will replace the current information.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(bool), 400)]
    public async Task<ActionResult> PutContact(int id, [FromBody]Contact contact)
    {
      if (!ModelState.IsValid)
        return BadRequest(this.ModelState);

      var status = await _repo.UpdateContactAsync(id, contact);
      if (!status)
        return BadRequest("Unable to update contact");
        
      return Ok(status);
    }


    // DELETE /contacts/<int:id>
    /// <summary>
    /// Deletes a contact in the database.
    /// </summary>
    /// <param name="id">The id of the contact to delete.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(bool), 404)]
    public async Task<ActionResult> DeleteContact(int id)
    {
      var status = await _repo.DeleteContactAsync(id);
      return !status
        ? NotFound()
        : Ok(status);
    }

}