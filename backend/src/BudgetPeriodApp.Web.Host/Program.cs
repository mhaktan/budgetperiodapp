using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BudgetPeriodApp.EntityFrameworkCore;
using BudgetPeriodApp.EntityFrameworkCore.Seed;
using BudgetPeriodApp.Entities;

namespace BudgetPeriodApp.Web.Host
{
    /// <summary>
    /// Background service that runs migration + seed once at startup
    /// without blocking the HTTP pipeline.
    /// </summary>
    public class MigrationHostedService : IHostedService
    {
        private readonly IConfiguration _config;

        public MigrationHostedService(IConfiguration config)
        {
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var connStr = _config.GetConnectionString("Default") ?? "";
            if (string.IsNullOrEmpty(connStr)) return;

            // Run in background to avoid blocking host startup (prevents EF tooling timeout)
            _ = Task.Run(async () =>
            {
                // Small delay to ensure host is fully started before DB operations
                await Task.Delay(1000, cancellationToken);
                try
                {
                    var optionsBuilder = new DbContextOptionsBuilder();
                    optionsBuilder.UseNpgsql(connStr);

                    using (var db = new BudgetPeriodAppDbContext(optionsBuilder.Options))
                    {
                        // EnsureCreated bos veritabaninda en guncel semayi kurar ve true doner.
                        // Dolu veritabaninda hicbir sey yapmaz — o durumda bekleyen migration'lar
                        // SchemaMigrations.Apply icinde sirayla calistirilir.
                        var freshlyCreated = db.Database.EnsureCreated();
                        Console.WriteLine("[Migration] Migration tanimi yok — sema EnsureCreated ile yonetiliyor.");
                        Console.WriteLine("[Migration] Database is up to date.");

                        // Seed sample data — wrapped in its own try so a failure here doesn't block RBAC seed below.
                        try
                        {
                    if (!db.BudgetPeriods.Any())
                    {
                        db.BudgetPeriods.AddRange(
                    new BudgetPeriod { Id = 1, Name = "Bütçe Dönemi 1", PeriodType = (BudgetPeriodPeriodType)0, StartDate = new DateTime(2024, 3, 15), EndDate = new DateTime(2024, 3, 15), Description = "Lorem ipsum dolor sit amet", IsActive = true },
                    new BudgetPeriod { Id = 2, Name = "Bütçe Dönemi 2", PeriodType = (BudgetPeriodPeriodType)1, StartDate = new DateTime(2024, 6, 20), EndDate = new DateTime(2024, 6, 20), Description = "Consectetur adipiscing elit", IsActive = false }
                        );
                    }
                    if (!db.Departments.Any())
                    {
                        db.Departments.AddRange(
                    new Department { Id = 3, Name = "Departman 1", Code = "ABC-001", Description = "Lorem ipsum dolor sit amet" },
                    new Department { Id = 4, Name = "Departman 2", Code = "XYZ-002", Description = "Consectetur adipiscing elit" }
                        );
                    }
                    if (!db.ExpenseCategories.Any())
                    {
                        db.ExpenseCategories.AddRange(
                    new ExpenseCategory { Id = 5, Name = "Harcama Kategorisi 1", Code = "ABC-001", Description = "Lorem ipsum dolor sit amet" },
                    new ExpenseCategory { Id = 6, Name = "Harcama Kategorisi 2", Code = "XYZ-002", Description = "Consectetur adipiscing elit" }
                        );
                    }
                    if (!db.Budgets.Any())
                    {
                        db.Budgets.AddRange(
                    new Budget { Id = 7, Title = "Introduction to Physics", TotalAmount = 99.99m, Description = "Lorem ipsum dolor sit amet", Status = (BudgetStatus)0, RevisionNote = "Lorem ipsum dolor sit amet", BudgetPeriodId = 1, DepartmentId = 3 },
                    new Budget { Id = 8, Title = "Advanced Mathematics", TotalAmount = 149.50m, Description = "Consectetur adipiscing elit", Status = (BudgetStatus)0, RevisionNote = "Consectetur adipiscing elit", BudgetPeriodId = 2, DepartmentId = 4 }
                        );
                    }
                    if (!db.BudgetLines.Any())
                    {
                        db.BudgetLines.AddRange(
                    new BudgetLine { Id = 9, Description = "Lorem ipsum dolor sit amet", PlannedAmount = 99.99m, Notes = "Lorem ipsum dolor sit amet", BudgetId = 7, ExpenseCategoryId = 5 },
                    new BudgetLine { Id = 10, Description = "Consectetur adipiscing elit", PlannedAmount = 149.50m, Notes = "Consectetur adipiscing elit", BudgetId = 8, ExpenseCategoryId = 6 }
                        );
                    }
                    if (!db.ExpenseRequests.Any())
                    {
                        db.ExpenseRequests.AddRange(
                    new ExpenseRequest { Id = 11, Title = "Introduction to Physics", Amount = 99.99m, RequestDate = new DateTime(2024, 3, 15), Description = "Lorem ipsum dolor sit amet", Status = (ExpenseRequestStatus)0, RevisionNote = "Lorem ipsum dolor sit amet", BudgetLineId = 9, ExpenseCategoryId = 5, DepartmentId = 3 },
                    new ExpenseRequest { Id = 12, Title = "Advanced Mathematics", Amount = 149.50m, RequestDate = new DateTime(2024, 6, 20), Description = "Consectetur adipiscing elit", Status = (ExpenseRequestStatus)0, RevisionNote = "Consectetur adipiscing elit", BudgetLineId = 10, ExpenseCategoryId = 6, DepartmentId = 4 }
                        );
                    }
                    if (!db.ActualExpenses.Any())
                    {
                        db.ActualExpenses.AddRange(
                    new ActualExpense { Id = 13, Description = "Lorem ipsum dolor sit amet", Amount = 99.99m, ExpenseDate = new DateTime(2024, 3, 15), Notes = "Lorem ipsum dolor sit amet", ExpenseRequestId = 11, ExpenseCategoryId = 5 },
                    new ActualExpense { Id = 14, Description = "Consectetur adipiscing elit", Amount = 149.50m, ExpenseDate = new DateTime(2024, 6, 20), Notes = "Consectetur adipiscing elit", ExpenseRequestId = 12, ExpenseCategoryId = 6 }
                        );
                    }
                            db.SaveChanges();
                            Console.WriteLine("[Seed] Sample data created.");
                        }
                        catch (Exception sampleEx)
                        {
                            Console.WriteLine($"[Seed] Sample data skipped: {sampleEx.GetType().Name}: {sampleEx.Message}");
                            // Carry on — RBAC seed must still run so admin/123qwe is usable.
                        }
                        // Sync identity sequences to MAX(Id). Seeded rows carry explicit Ids which do NOT
                        // advance Postgres identity sequences → nextval collides with a seed row and the
                        // first few inserts fail with a duplicate-key 500. Runs every startup; idempotent.
                        try
                        {
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"BudgetPeriods\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"BudgetPeriods\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"Departments\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"Departments\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"ExpenseCategories\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"ExpenseCategories\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"Budgets\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"Budgets\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"BudgetLines\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"BudgetLines\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"ExpenseRequests\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"ExpenseRequests\") + 1, false);");
                            db.Database.ExecuteSqlRaw("SELECT setval(pg_get_serial_sequence('\"ActualExpenses\"', 'Id'), (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"ActualExpenses\") + 1, false);");
                            Console.WriteLine("[Seed] Identity sequences synced.");
                        }
                        catch (Exception seqEx)
                        {
                            Console.WriteLine($"[Seed] Sequence sync skipped: {seqEx.GetType().Name}: {seqEx.Message}");
                        }
                    }
                    // RBAC seed (Admin/User roles + permissions + admin user) runs through ABP DI
                    // so PermissionRegistry can be injected. SeedHelper is idempotent.
                    SeedHelper.SeedHostDb(Abp.Dependency.IocManager.Instance);
                    Console.WriteLine("[Seed] RBAC seed complete (Admin role + admin user).");
                }
                catch (Exception ex)
                {
                    // Full diagnostic — surface the real cause so silent seed failures are debuggable.
                    Console.WriteLine($"[Migration] FAILED: {ex.GetType().Name}: {ex.Message}");
                    if (ex.InnerException != null)
                        Console.WriteLine($"[Migration] InnerException: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
                    Console.WriteLine("[Migration] StackTrace:");
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("[Migration] App continues without migration — admin user will not exist.");
                }
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public class Program
    {
        // Runtime entry: WebHost is required because ABP Startup returns IServiceProvider.
        public static void Main(string[] args)
        {
            // Npgsql 7+ requires UTC DateTimes — enable legacy behavior for ABP compatibility
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .Run();
        }

        // Design-time entry for EF Core tools (dotnet ef migrations).
        // Without this, EF tools wait 5 minutes for IHost build (resolver default timeout)
        // and then SIGTERM any running dotnet process — killing live dev servers.
        // We expose a minimal IHost that EF tools resolve in milliseconds; the actual
        // DbContext is built by IDesignTimeDbContextFactory in the EntityFrameworkCore project.
        public static IHostBuilder CreateHostBuilder(string[] args)
            => Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);
    }
}
