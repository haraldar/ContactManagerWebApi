using Microsoft.EntityFrameworkCore;
using ContactManagerWebApi.Models;

namespace ContactManagerWebApi.Data;

/// <summary>
/// Handles the initial population of the database.
/// </summary>
public class ContactsDbSeeder
{
    readonly ILogger _logger;


    public ContactsDbSeeder(ILoggerFactory loggerFactory)
        => _logger = loggerFactory.CreateLogger("ContactsDbSeederLogger");


    /// <summary>
    /// Handles the creation and migration of the database including the initial populating of seed data.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public async Task SeedAsync(IServiceProvider serviceProvider)
    {
        // 
        using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var contactsDb = serviceScope.ServiceProvider.GetService<ContactsDbContext>();
            await contactsDb.Database.MigrateAsync();
            // if (await contactsDb.Database.EnsureCreatedAsync())
            // {
                if (!await contactsDb.Contacts.AnyAsync()) {
                    await InsertContactsSampleData(contactsDb);
                }
            // }
        }

    }


    /// <summary>
    /// Inserts the sample contact data into the database.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <returns></returns>
    public async Task InsertContactsSampleData(ContactsDbContext context)
    {
        // Fetch the sample contacts and add them into the database context.
        var contacts = GetSampleContacts();
        context.Contacts.AddRange(contacts);

        // Try to save the sample contacts to the database.
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(ContactsDbSeeder)}: " + ex.Message);
            throw;
        }

    }


    /// <summary>
    /// Just contains a list of sample data to populate the database with.
    /// </summary>
    /// <returns>A list of sample contacts.</returns>
    private List<Contact> GetSampleContacts()
        => new List<Contact>()
            {
                new Contact()
                {
                    Salution = "Frau",
                    FirstName = "Nikita",
                    LastName = "Khutorni",
                    DisplayName = "",
                    BirthDate = new DateTime(1998, 6, 4).ToUniversalTime(),
                    Email = "nkhutorni@protonmail.com",
                    PhoneNumber = "0176/123456789"
                }
            };

}