using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SimpleApi.Data
{
    using SimpleApi.src.Models;
    public class IdentityAppDbContext : IdentityDbContext<ApplicationUser>
    {
    public IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options)
        : base(options)
    {
    }

    }


}