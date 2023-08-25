using LeaveManagmentSystemAPI.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagmentSystemAPI
{
    public static class SeedData
    {
        public static void Seed(UserManager<Employee> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<Employee> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var user = new Employee
                {
                    Firstname = "Admin",
                    Lastname = "Admin",
                    UserName = "admin",
                    Email = "admin@localhost.com"
                };
                var result = userManager.CreateAsync(user, "Abc@1234").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
            if (userManager.FindByNameAsync("employee").Result == null)
            {
                var user = new Employee
                {
                    Firstname = "Nouman",
                    Lastname = "Arshad",
                    UserName = "employee",
                    Email = "employee@localhost.com"
                };
                var result = userManager.CreateAsync(user, "Abc@1234").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                var result = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Employee"
                };
                var result = roleManager.CreateAsync(role).Result;
            }
        }

    }
}
