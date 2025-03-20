using CreateContact.Application.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CreateContact.Infrastructure.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ContactRepository(ApplicationDbContext dbContext) : base()
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> IsDddAndPhoneUniqueAsync(int ddd, string phone)
        {
            return await _dbContext.Contacts.AnyAsync(c =>
                            c.Region.DddCode == ddd
                            && c.Phone == phone);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _dbContext.Contacts.AnyAsync(c => c.Email == email);
        }
    }
}
