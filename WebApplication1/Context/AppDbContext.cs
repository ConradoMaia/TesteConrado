using Microsoft.EntityFrameworkCore;
using CadastroDeUsinas.Models;

namespace CadastroDeUsinas.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Usina> Usinas { get; set; }
    }
}
