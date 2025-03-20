using CreateContact.Domain.Exceptions;
using CreateContact.Domain.ValueObjects;
using System.Text.RegularExpressions;

namespace CreateContact.Domain.Entities;

public class Contact
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; private set; }
    public Region Region { get; private set; }
    public string Phone { get; private set; }
    public string? Email { get; private set; }

    // For Entity Framework Core - Migration updated needed
    protected Contact() { }

    public static Contact Create(string name, int dddCode, string phone, string? email = null)
    {
        ValidateName(name);
        ValidatePhone(phone);
        ValidateEmail(email);

        return new Contact
        {
            Name = name,
            Region = Region.Create(dddCode),
            Phone = phone,
            Email = email
        };
    }

    public void UpdateName(string newName)
    {
        ValidateName(newName);
        Name = newName;
    }

    public void UpdateRegion(int newDddCode)
    {
        Region = Region.Create(newDddCode);
    }

    public void UpdatePhone(string newPhone)
    {
        ValidatePhone(newPhone);
        Phone = newPhone;
    }

    public void UpdateEmail(string newEmail)
    {
        ValidateEmail(newEmail);
        Email = newEmail;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidNameException();
    }

    private static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone) || phone.Length != 9 || !phone.All(char.IsDigit))
            throw new InvalidPhoneNumberException(phone);
    }

    private static void ValidateEmail(string? email)
    {
        if (email is not null && !IsValidEmailFormat(email))
            throw new InvalidEmailException(email);
    }

    private static bool IsValidEmailFormat(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}
