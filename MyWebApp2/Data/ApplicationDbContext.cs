using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyWebApp2.Models;

namespace MyWebApp2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MyWebApp2.Models.Vocstack> Vocstack { get; set; }
        public DbSet<MyWebApp2.Models.Voc> Voc { get; set; }
        public DbSet<MyWebApp2.Models.ScBoard> ScBoard { get; set; }


    }
}