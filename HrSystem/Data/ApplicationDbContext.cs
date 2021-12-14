using HrSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HrSystem.ViewModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HrSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, UserRole, string>  
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
        public virtual DbSet<ExtraDiscountSetting> ExtraDiscountSettings { get; set; }
        public virtual DbSet<WeeklyHoliday> WeeklyHolidays { get; set; }
        public virtual DbSet<OfficialHoliday> OfficialHolidays { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Employee>().ToTable("Employees");
            builder.Entity<User>().ToTable("Users");
            builder.Entity<UserRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UsersRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UsersClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UsersTokens");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UsersLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<Salary>().HasKey(m => new { m.Year, m.Month, m.employeeId });

        }

        //protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        //{
        //    builder.Properties<DateOnly>()
        //        .HaveConversion<DateOnlyConverter>()
        //        .HaveColumnType("date");
        //}
        //public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        //{
        //    public DateOnlyConverter() : base(
        //            d => d.ToDateTime(TimeOnly.MinValue),
        //            d => DateOnly.FromDateTime(d))
        //    { }
        //}
    }
}