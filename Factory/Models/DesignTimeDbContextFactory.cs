using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DocOffice.Models
{
  public class DocOfficeContextFactory: IDesignTimeDbContextFactory<DocOfficeContext>
  {
    DocOfficeContext IDesignTimeDbContextFactory<DocOfficeContext>.CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

      var builder = new DbContextOptionsBuilder<DocOfficeContext>();

      builder.UseMySql(configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(configuration["COnnectionStrings:DefaultConnection"]));

      return new DocOfficeContext(builder.Options);
    }
  }
}