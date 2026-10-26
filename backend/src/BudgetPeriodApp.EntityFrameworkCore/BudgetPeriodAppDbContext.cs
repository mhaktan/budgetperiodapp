using Microsoft.EntityFrameworkCore;
using Abp.EntityFrameworkCore;
using BudgetPeriodApp.Entities;

namespace BudgetPeriodApp.EntityFrameworkCore
{
    public class BudgetPeriodAppDbContext : AbpDbContext
    {
        public DbSet<BudgetPeriod> BudgetPeriods { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetLine> BudgetLines { get; set; }
        public DbSet<ExpenseRequest> ExpenseRequests { get; set; }
        public DbSet<ActualExpense> ActualExpenses { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<ApprovalRecord> ApprovalRecords { get; set; }
        public DbSet<StatusChangeLog> StatusChangeLogs { get; set; }


        public BudgetPeriodAppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // BudgetPeriod 1:N Budget
            modelBuilder.Entity<Budget>()
                .HasOne(x => x.BudgetPeriod)
                .WithMany(x => x.Budgets)
                .HasForeignKey(x => x.BudgetPeriodId)
                .OnDelete(DeleteBehavior.Restrict);

            // Department 1:N Budget
            modelBuilder.Entity<Budget>()
                .HasOne(x => x.Department)
                .WithMany(x => x.Budgets)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Budget 1:N BudgetLine
            modelBuilder.Entity<BudgetLine>()
                .HasOne(x => x.Budget)
                .WithMany(x => x.BudgetLines)
                .HasForeignKey(x => x.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            // ExpenseCategory 1:N BudgetLine
            modelBuilder.Entity<BudgetLine>()
                .HasOne(x => x.ExpenseCategory)
                .WithMany(x => x.BudgetLines)
                .HasForeignKey(x => x.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // BudgetLine 1:N ExpenseRequest
            modelBuilder.Entity<ExpenseRequest>()
                .HasOne(x => x.BudgetLine)
                .WithMany(x => x.ExpenseRequests)
                .HasForeignKey(x => x.BudgetLineId)
                .OnDelete(DeleteBehavior.Restrict);

            // ExpenseCategory 1:N ExpenseRequest
            modelBuilder.Entity<ExpenseRequest>()
                .HasOne(x => x.ExpenseCategory)
                .WithMany(x => x.ExpenseRequests)
                .HasForeignKey(x => x.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Department 1:N ExpenseRequest
            modelBuilder.Entity<ExpenseRequest>()
                .HasOne(x => x.Department)
                .WithMany(x => x.ExpenseRequests)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ExpenseRequest 1:N ActualExpense
            modelBuilder.Entity<ActualExpense>()
                .HasOne(x => x.ExpenseRequest)
                .WithMany(x => x.ActualExpenses)
                .HasForeignKey(x => x.ExpenseRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            // ExpenseCategory 1:N ActualExpense
            modelBuilder.Entity<ActualExpense>()
                .HasOne(x => x.ExpenseCategory)
                .WithMany(x => x.ActualExpenses)
                .HasForeignKey(x => x.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            // RBAC: AppUser N:N AppRole via UserRole junction
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRole>()
                .HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique();

            // RolePermission: AppRole 1:N RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RolePermission>()
                .HasIndex(rp => new { rp.RoleId, rp.PermissionName })
                .IsUnique();

            // AppRole.Name unique
            modelBuilder.Entity<AppRole>()
                .HasIndex(r => r.Name)
                .IsUnique();

        }
    }
}
