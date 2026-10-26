using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BudgetPeriodApp.EntityFrameworkCore
{
    public class BudgetPeriodAppDbContextFactory : IDesignTimeDbContextFactory<BudgetPeriodAppDbContext>
    {
        public BudgetPeriodAppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BudgetPeriodApp.Web.Host"))
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connStr = configuration.GetConnectionString("Default");
            var builder = new DbContextOptionsBuilder();
            builder.UseNpgsql(connStr);
            return new BudgetPeriodAppDbContext(builder.Options);
        }
    }
}
