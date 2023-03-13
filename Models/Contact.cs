using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ContactManagerWebApi.Models;

[Table("contacts")]
public class Contact
{

    ///<summary>The autoincrementing primary identification key.</summary>///
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    ///<summary>The contact's salution.</summary>///
    [Required, MinLength(2)]
    public string Salution { get; set; }
    
    ///<summary>The contact's first name.</summary>///
    [Required, MinLength(2)]
    public string FirstName { get; set; }

    ///<summary>The contact's last name.</summary>///
    [Required, MinLength(2)]
    public string LastName { get; set; }

    private string _displayName;
    ///<summary>The contact's display name.</summary>///
    public string DisplayName
    {
        get => string.IsNullOrEmpty(_displayName)
            ? $"{Salution} {FirstName} {LastName}"
            : _displayName;
        set =>  _displayName = value;
    }

    ///<summary>The contact's birth date.</summary>///
    private DateTime _birthDate;
    public DateTime BirthDate
    {
        get => _birthDate; 
        set => _birthDate = value.ToUniversalTime();
    }

    ///<summary>The contact's unique email.</summary>///
    [Required, EmailAddress]
    public string Email { get; set; }

    ///<summary>The contact's phone number.</summary>///
    public string PhoneNumber { get; set; }

    ///<summary>The contact's creation date and time.</summary>///
    public DateTime CreationTimestamp { get; }

    ///<summary>The last time the contact changed any of his data.</summary>///
    public DateTime LastChangeTimestamp { get; private set; }

    private bool _notifyHasBirthdaySoon;
    ///<summary>A checker if the contact will celebrate his birthday soon.</summary>///
    [NotMapped]
    public bool? NotifyHasBirthdaySoon {
        get {
            if (BirthDate != null) {
                // Set today, caculate the date in 14 days and the birthdate this year.
                var today = DateTime.Now.Date;
                var todayInTwoWeeks = today.AddDays(14);
                var thisBirthDate = new DateTime(today.Year, BirthDate.Month, BirthDate.Day);

                // If this birthdate is 
                if (thisBirthDate < today)
                {
                    DateTime nextYearBirthday = new DateTime(today.Year + 1, BirthDate.Month, BirthDate.Day);
                    bool isLeapYear = nextYearBirthday - TimeSpan.FromDays(365) == BirthDate;
                    thisBirthDate = isLeapYear ? nextYearBirthday.AddDays(-1) : nextYearBirthday;
                }

                // Check if the birthday is inbetween today and today in two weeks.
                return today <= thisBirthDate && thisBirthDate < todayInTwoWeeks;
            }
            else {
                return null;
            }
        }
    }

}
