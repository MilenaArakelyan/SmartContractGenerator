using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectS5.Data;

public class ProjectS5DbContextFactory : IDesignTimeDbContextFactory<ProjectS5DbContext>
{
    public ProjectS5DbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProjectS5DbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;");

        return new ProjectS5DbContext(optionsBuilder.Options);
    }
}
