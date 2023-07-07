using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Seeding
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> manager, RoleManager<AppRole> roleManager)
         {
             if (await manager.Users.AnyAsync()) return;
             var userData = await System.IO.File.ReadAllTextAsync("Seeding/UserSeedData.json");
             var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
             if (users == null) return;

             var roles = new List<AppRole>{
                 new AppRole{Name = "Surgery"},
                 new AppRole{Name = "Moderator"},
                 new AppRole{Name = "Sponsor"},
                 new AppRole{Name = "Refcard"},
                 new AppRole{Name = "Admin"},
                 new AppRole{Name = "Cardiologist"},
                 new AppRole{Name = "Chef"}
             };
             foreach (var role in roles) { await roleManager.CreateAsync(role); }
             foreach (AppUser ap in users)
             {
                 ap.UserName = ap.UserName.ToLower();
                 ap.Country = "NL";
                 await manager.CreateAsync(ap, "Pa$$w0rd");
                 await manager.AddToRoleAsync(ap, "Surgery");
             }

             var admin = new AppUser{
                  UserName = "admin@gfancy.nl",
                  Email = "admin@gfancy.nl",
                  Gender = "male",
                  ltk = false,
                  PaidTill = new DateTime().AddYears(2250),
                  KnownAs = "the boss",
                  active = true,
                  Country = "NL" };
             await manager.CreateAsync(admin, "Pa$$w0rd");
             await manager.AddToRoleAsync(admin, "Admin");
           


         } 
        public static async Task SeedEmployees(DataContext context)
        {
            if (await context.Employees.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Seeding/employeeSeed.json");
            var emp = JsonSerializer.Deserialize<List<Class_Employee>>(userData);
            foreach (var item in emp)
            {
                item.user_name = item.user_name.ToLower();
                context.Employees.Add(item);
            }
            await context.SaveChangesAsync();
        }
        public static async Task SeedHospitals(DataContext context)
        {
            if (await context.Hospitals.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Seeding/hospitalSeedData.json");
            var emp = JsonSerializer.Deserialize<List<Class_Hospital>>(userData);
            foreach (var item in emp) { context.Hospitals.Add(item); }
            await context.SaveChangesAsync();

        }
        public static async Task SeedPatients(DataContext context)
        {
            if (await context.Patients.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Seeding/patientSeedData.json");
            var emp = JsonSerializer.Deserialize<List<Class_Patient>>(userData);
            foreach (var item in emp) { context.Patients.Add(item); }
            await context.SaveChangesAsync();

        }
        public static async Task SeedProcedures(DataContext context)
        {

            if (await context.Procedures.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Seeding/procedureSeedData.json");
            var emp = JsonSerializer.Deserialize<List<Class_Procedure>>(userData);
            foreach (var item in emp) { context.Procedures.Add(item); }
            await context.SaveChangesAsync();

        }
       /*   public static async Task SeedValvesInHospital(DataContext context)
        {
            if(await context.ValveCodes.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("data/seed/valvesInHospital.json");
            var emp = JsonSerializer.Deserialize<List<Class_Valve_Code>>(userData);
            foreach (var item in emp)
            {
               context.ValveCodes.Add(item);
            }
            await context.SaveChangesAsync();
        }  */

         public static async Task SeedRefPhys(DataContext context)
          {
              if (await context.RefPhys.AnyAsync()) return;

              var userData = await System.IO.File.ReadAllTextAsync("Seeding/refCard.json");
              var emp = JsonSerializer.Deserialize<List<Class_Ref_Phys>>(userData);
              foreach (var item in emp) { context.RefPhys.Add(item); }
              await context.SaveChangesAsync();
          }
        /*  public static async Task SeedEpaas(UserManager<AppUser> manager)
          {
              var listOfEpas = new List<Class_Epa>();
              var user = await manager.Users.Include(x => x.Epa).FirstOrDefaultAsync(x => x.Id == 2);
              listOfEpas = user.Epa.ToList();
              if (listOfEpas.Count != 0) return;

              var userData = await System.IO.File.ReadAllTextAsync("Seeding/epaSeedData.json");
              var emp = JsonSerializer.Deserialize<List<Class_Epa>>(userData);
              foreach (var item in emp) { user.Epa.Add(item); }
              await manager.UpdateAsync(user);
          }
         public static async Task SeedCourses(UserManager<AppUser> manager)
          {
              var listOfCourses = new List<Class_Course>();
              var user = await manager.Users.Include(x => x.Courses).FirstOrDefaultAsync(x => x.Id == 2);
              listOfCourses = user.Courses.ToList();
              if (listOfCourses.Count != 0) return;

              var userData = await System.IO.File.ReadAllTextAsync("Seeding/courseSeedData.json");
              var emp = JsonSerializer.Deserialize<List<Class_Course>>(userData);
              foreach (var item in emp) { user.Courses.Add(item); }
              await manager.UpdateAsync(user);
          } */
    }
}