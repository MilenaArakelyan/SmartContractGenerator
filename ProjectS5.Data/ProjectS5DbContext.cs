using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjectS5.Data;

public class ProjectS5DbContext : IdentityDbContext<ApplicationUser>
{
    public ProjectS5DbContext(DbContextOptions<ProjectS5DbContext> options) : base(options)
    {

    }
}