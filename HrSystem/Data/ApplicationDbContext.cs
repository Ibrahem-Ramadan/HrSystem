using HrSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HrSystem.ViewModels;

namespace HrSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<Employee,EmployeeRole,string>  
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Vacation> Vacations { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Permissions> Permissions { get; set; }
        public virtual DbSet<RolesPermession> RolesPermessions { get; set; }
        public virtual DbSet<ExtraDiscountSetting> ExtraDiscountSettings { get; set; }
        public virtual DbSet<WeeklyHoliday> WeeklyHolidays { get; set; }
        
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Employee>().ToTable("Employees");
            builder.Entity<EmployeeRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UsersRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UsersClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UsersTokens");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UsersLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<Salary>().HasKey(m => new { m.Year, m.Month, m.employeeId });

            builder.Entity<RolesPermession>().HasKey(c => new { c.RoleId, c.PermissionId });

        }
        
       
        public DbSet<HrSystem.ViewModels.EmployeeVM> EmployeeVM { get; set; }
        
       
        public DbSet<HrSystem.ViewModels.EmployeeViewModel> EmployeeViewModel { get; set; }
    }
}