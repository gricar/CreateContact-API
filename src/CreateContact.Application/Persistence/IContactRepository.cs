namespace CreateContact.Application.Persistence;

public interface IContactRepository
{
    Task<bool> IsEmailUniqueAsync(string email);
    Task<bool> IsDddAndPhoneUniqueAsync(int ddd, string phone);
}
