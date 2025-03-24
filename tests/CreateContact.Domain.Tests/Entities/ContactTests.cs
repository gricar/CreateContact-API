using CreateContact.Domain.Entities;
using CreateContact.Domain.Exceptions;
using CreateContact.Domain.Tests.Fixtures;
using FluentAssertions;

namespace CreateContact.Domain.Tests.Entities;

[Collection(nameof(ContactFixtureCollection))]
public class ContactTests
{
    private readonly ContactFixture _fixture;

    public ContactTests(ContactFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "Should create new contact with valid values")]
    [Trait("Category", "Create Contact - Success")]
    public void CreateContact_ValidParameters_ShouldCreate()
    {
        // Arrange
        var contact = _fixture.CreateValidContact();

        // Assert
        contact.Should().NotBeNull();
        contact.Name.Should().NotBeNullOrWhiteSpace();
        contact.Name.Should().Be(contact.Name);
        contact.Region.DddCode.Should().BeGreaterThan(0);
        contact.Region.DddCode.Should().Be(contact.Region.DddCode);
        contact.Phone.Should().MatchRegex(@"^\d{9}$");
        contact.Phone.Should().Be(contact.Phone);
        contact.Email.Should().MatchRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        contact.Email.Should().Be(contact.Email);
    }

    [Theory(DisplayName = "Should throw exception when creating new contact with invalid values")]
    [Trait("Category", "Create Contact - Failure")]
    [InlineData("", 11, "987654321", "jony@example.com", "Name is required.")]
    [InlineData(null, 11, "987654321", "jony@example.com", "Name is required.")]
    [InlineData("Jony", 11, null, "jony@example.com", "Phone '' must be a 9-digit number.")]
    [InlineData("Jony", 11, "12345", "jony@example.com", "Phone '12345' must be a 9-digit number.")]
    [InlineData("Jony", 11, "1a3b56789", "jony@example.com", "Phone '1a3b56789' must be a 9-digit number.")]
    [InlineData("Jony", 11, "987654321", "", "Email '' must be a valid format.")]
    [InlineData("Jony", 11, "987654321", "jony@invalid", "Email 'jony@invalid' must be a valid format.")]
    [InlineData("Jony", 11, "987654321", "jony.com", "Email 'jony.com' must be a valid format.")]

    public void CreateContact_InvalidParameters_ShouldThrowException(
        string name, int dddCode, string phone, string email, string expectedErrorMessage)
    {
        // Act
        Action act = () => Contact.Create(name, dddCode, phone, email);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage(expectedErrorMessage);
    }

    [Fact(DisplayName = "Should update name when valid name is provided")]
    [Trait("Category", "Update Contact Name - Success")]
    public void UpdateContact_ValidName_ShouldUpdateName()
    {
        // Arrange
        var contact = _fixture.CreateValidContact();
        string newName = "Jane";

        // Act
        contact.UpdateName(newName);

        // Assert
        contact.Name.Should().Be(newName);
    }

    [Theory(DisplayName = "Should throw exception when updating name with invalid value")]
    [Trait("Category", "Update Contact Name - Failure")]
    [InlineData(null)]
    [InlineData("")]
    public void UpdateContact_InvalidName_ShouldThrowException(string invalidName)
    {
        // Arrange
        var contact = _fixture.CreateValidContact();

        // Act
        Action act = () => contact.UpdateName(invalidName);

        // Assert
        act.Should().Throw<InvalidNameException>()
            .WithMessage("Name is required.");
    }

    [Fact(DisplayName = "Should update phone when valid phone is provided")]
    [Trait("Category", "Update Contact Phone - Success")]
    public void UpdateContact_ValidPhone_ShouldUpdatePhone()
    {
        // Arrange
        var contact = _fixture.CreateValidContact();
        string newPhone = "987654321";

        // Act
        contact.UpdatePhone(newPhone);

        // Assert
        contact.Phone.Should().Be(newPhone);
    }

    [Theory(DisplayName = "Should throw exception when updating phone with invalid value")]
    [Trait("Category", "Update Contact Phone - Failure")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("12345678910")]
    [InlineData("1a3456b89")]
    public void UpdateContact_InvalidPhones_ShouldThrowException(string invalidPhone)
    {
        // Arrange
        var contact = _fixture.CreateValidContact();

        // Act
        Action act = () => contact.UpdatePhone(invalidPhone);

        // Assert
        act.Should().Throw<InvalidPhoneNumberException>()
            .WithMessage($"Phone '{invalidPhone}' must be a 9-digit number.");
    }

    [Fact(DisplayName = "Should update email when valid email is provided")]
    [Trait("Category", "Update Contact Email - Success")]
    public void UpdateContact_ValidEmail_ShouldUpdateEmail()
    {
        // Arrange
        var contact = _fixture.CreateValidContact();

        string newEmail = "jony@example.com";

        // Act
        contact.UpdateEmail(newEmail);

        // Assert
        contact.Email.Should().Be(newEmail);
        contact.Email.Should().NotBeNull();
    }

    [Theory(DisplayName = "Should throw exception when updating email with invalid value")]
    [Trait("Category", "Update Contact Email - Failure")]
    [InlineData("jony@example")]
    public void UpdateContact_InvalidEmails_ShouldThrowException(string invalidEmail)
    {
        // Arrange
        var contact = Contact.Create("Jony", 11, "123456789", null);

        // Act
        Action act = () => contact.UpdateEmail(invalidEmail);

        // Assert
        act.Should().Throw<InvalidEmailException>()
            .WithMessage($"Email '{invalidEmail}' must be a valid format.");
    }
}
