using Microsoft.EntityFrameworkCore;
 
namespace ReceiptScanner.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        // public DbSet<Users> users{get;set;}
        public DbSet<Photos> photos{get;set;}
    }
}