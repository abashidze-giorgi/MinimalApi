using Microsoft.EntityFrameworkCore;

namespace MinimalApi
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<SuperHero> SuperHeroes { get; set; }


    }
}
