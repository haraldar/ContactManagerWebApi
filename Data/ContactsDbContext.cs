using Microsoft.EntityFrameworkCore;
using ContactManagerWebApi.Models;
using Microsoft.Extensions.Hosting;

namespace ContactManagerWebApi.Data;
/// <summary>
/// Handles the database session and the database data model and its configuration.
/// </summary>
public class ContactsDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }

    public ContactsDbContext (DbContextOptions<ContactsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var contacts = modelBuilder.Entity<Contact>();

        contacts.HasKey(c => c.Id);
        contacts
            .Property(c => c.Id)
            .UseIdentityAlwaysColumn();

        contacts
            .Property(c => c.CreationTimestamp)
            .HasDefaultValueSql("current_timestamp");

        contacts
            .Property(c => c.LastChangeTimestamp)
            .HasDefaultValueSql("current_timestamp")
            .ValueGeneratedOnAddOrUpdate();

    }
}