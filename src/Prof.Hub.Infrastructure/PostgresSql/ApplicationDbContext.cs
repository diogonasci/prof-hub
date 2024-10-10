using Microsoft.EntityFrameworkCore;
using Prof.Hub.Domain.Entities;

namespace Prof.Hub.Infrastructure.PostgresSql;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Student> Students { get; set; }
}
