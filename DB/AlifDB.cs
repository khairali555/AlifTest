using AlifTestTask.Models;
using Microsoft.EntityFrameworkCore;

namespace AlifTestTask.DB
{
    public class AlifDB: DbContext
    {
        public AlifDB()
        {
        }

        public AlifDB(DbContextOptions<AlifDB> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
    }
}
