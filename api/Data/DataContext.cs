using api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{

    public class DataContext : IdentityDbContext<
    AppUser, 
    AppRole, 
    int, 
    IdentityUserClaim<int>,
    AppUserRole,
    IdentityUserLogin<int>,
    IdentityRoleClaim<int>, 
    IdentityUserToken<int>
    >
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Class_Employee> Employees { get; set; }
        public DbSet<Class_Patient> Patients { get; set; }
        public DbSet<Class_Procedure> Procedures { get; set; }
        public DbSet<Class_Valve> Valves { get; set; }
        public DbSet<Class_CABG> CABGS { get; set; }
        public DbSet<Class_CPB> CPBS { get; set; }
        public DbSet<Class_PostOp> PostOps { get; set; }
        public DbSet<Class_Ref_Phys> RefPhys { get; set; }
        public DbSet<Class_Preview_Operative_report> Previews { get; set; }
        public DbSet<Class_Aortic_Surgery> AorticSurgeries { get; set; }
        public DbSet<Class_minInv> MinInvs { get; set; }
        public DbSet<ClassTableVlad> Vlads { get; set; }
        public DbSet<Class_LTX> LTXs { get; set; }
        public DbSet<Class_User_Online> online_users {get; set;}
 


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

             builder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();
        }
    }
}
