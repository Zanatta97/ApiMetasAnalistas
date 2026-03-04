using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<Analyst> Analysts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Occurrence> Occurrences { get; set; }
        }
}
