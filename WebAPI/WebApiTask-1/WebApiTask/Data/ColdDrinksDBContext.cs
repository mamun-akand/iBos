using Microsoft.EntityFrameworkCore;
using WebApiTask.Models;

namespace WebApiTask.Data
{
    public class ColdDrinksDBContext : DbContext
    {
        public ColdDrinksDBContext(DbContextOptions<ColdDrinksDBContext> options) : base(options)
        {

        }
        public virtual DbSet<ColdDrinks> tblColdDrinks { get; set; }
    }
}
