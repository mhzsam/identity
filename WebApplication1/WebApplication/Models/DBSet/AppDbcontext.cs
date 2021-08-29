using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Models.DBSet
{
    public class AppDbcontext: IdentityDbContext
    {
        public AppDbcontext(DbContextOptions<AppDbcontext>options):base(options){
            
        }


        public DbSet<Emploee> Emploees { get; set; }
        
    }
}