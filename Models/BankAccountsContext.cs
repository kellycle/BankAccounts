using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Models
{
    public class BankAccountsContext : DbContext
    {
        public BankAccountsContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get;set;}
    }
}