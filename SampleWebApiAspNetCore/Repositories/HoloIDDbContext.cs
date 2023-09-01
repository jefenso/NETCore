using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class HoloIDDbContext : DbContext
    {
        public HoloIDDbContext(DbContextOptions<HoloIDDbContext> options)
            : base(options)
        {
        }

        public DbSet<HoloIDEntity> HoloIDItems { get; set; } = null!;
    }
}
