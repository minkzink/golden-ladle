﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenLadle.Models;

namespace GoldenLadle.Data
{
    public static class UserRoleInitializer
    {
        public static void SeedData(UserManager<User> userManager,RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("NormalUser").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "NormalUser";
                role.NormalizedName = "Normal User";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                role.NormalizedName = "Administrator";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
        public static void SeedUsers(UserManager<User> userManager)
        {
            if (userManager.FindByNameAsync("user").Result == null)
            {
                User user = new User();
                user.UserName = "user@example.com";
                user.Email = "user@example.com";
                user.FirstName = "John";
                user.LastName = "Smith";

                IdentityResult result = userManager.CreateAsync(user, "AVI4lyfe$").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "NormalUser").Wait();
                }
            }

            if (userManager.FindByNameAsync("admin").Result == null)
            {
                User user = new User();
                user.UserName = "admin@example.com";
                user.Email = "admin@example.com";
                user.FirstName = "Ben";
                user.LastName = "Rinehart";

                IdentityResult result = userManager.CreateAsync(user, "AVI4lyfe$").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
