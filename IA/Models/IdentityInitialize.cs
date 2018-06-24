using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using IA.Controllers;

namespace IA.Models
{
    public static class IdentityInitialize
    {
        // Load user accounts
        public static async void LoadUserAccounts()
        {
            // Get a reference to the objects we need
            var ds = new ApplicationDbContext();
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(ds));

            // Add the user(s) that the app needs when loaded for the first time
            // Change any of the data below to better match your app's needs
            if (userManager.Users.Count() == 0)
            {
                // User account manager...
                var uam = new ApplicationUser { UserName = "uam@example.com", Email = "uam@example.com" };
                var uamResult = await userManager.CreateAsync(uam, "Password123!");
                if (uamResult.Succeeded)
                {
                    // Add claims
                    await userManager.AddClaimAsync(uam.Id, new Claim(ClaimTypes.Email, "uam@example.com"));
                    await userManager.AddClaimAsync(uam.Id, new Claim(ClaimTypes.Role, "UserAccountManager"));
                    await userManager.AddClaimAsync(uam.Id, new Claim(ClaimTypes.GivenName, "User Account"));
                    await userManager.AddClaimAsync(uam.Id, new Claim(ClaimTypes.Surname, "Manager"));
                }

                // Developer/programmer...
                var dev = new ApplicationUser { UserName = "dev@example.com", Email = "dev@example.com" };
                var devResult = await userManager.CreateAsync(dev, "Password123!");
                if (devResult.Succeeded)
                {
                    // Add claims
                    await userManager.AddClaimAsync(dev.Id, new Claim(ClaimTypes.Email, "dev@example.com"));
                    await userManager.AddClaimAsync(dev.Id, new Claim(ClaimTypes.Role, "Developer"));
                    await userManager.AddClaimAsync(dev.Id, new Claim(ClaimTypes.GivenName, "App"));
                    await userManager.AddClaimAsync(dev.Id, new Claim(ClaimTypes.Surname, "Developer"));
                }

                // Me...
                var mmarangoni1 = new ApplicationUser { UserName = "mmarangoni1@example.com", Email = "mmarangoni1@example.com" };
                var mmarangoni1Result = await userManager.CreateAsync(mmarangoni1, "Password123!");
                if (mmarangoni1Result.Succeeded)
                {
                    // Add claims
                    await userManager.AddClaimAsync(mmarangoni1.Id, new Claim(ClaimTypes.Email, "mmarangoni1@example.com"));
                    await userManager.AddClaimAsync(mmarangoni1.Id, new Claim(ClaimTypes.Role, "Developer"));
                    await userManager.AddClaimAsync(mmarangoni1.Id, new Claim(ClaimTypes.GivenName, "Matthew"));
                    await userManager.AddClaimAsync(mmarangoni1.Id, new Claim(ClaimTypes.Surname, "Marangoni"));
                }
            }
        }

        // Load app claims

        public static void LoadAppClaims()
        {
            // Get a reference to the manager
            Manager m = new Manager();

            // If there are no claims, add them
            if (m.AppClaimGetAll().Count() == 0)
            {
                // Add the app's allowed claims here

                // Employee Role
                var employee = new AppClaimAdd();
                employee.ClaimType = ClaimTypes.Role;
                employee.ClaimValue = "Employee";
                employee.Description = "Assigned to accounts for employees of the music business";
                m.AppClaimAdd(employee);

                // Customer Role
                var customer = new AppClaimAdd();
                customer.ClaimType = ClaimTypes.Role;
                customer.ClaimValue = "Customer";
                customer.Description = "Assigned to accounts for customers of the music business";
                m.AppClaimAdd(customer);

                // Department OU's
                var sales = new AppClaimAdd();
                sales.ClaimType = "OU";
                sales.ClaimValue = "Sales";
                sales.Description = "Assigned to employee accounts who work in the sales department";
                m.AppClaimAdd(sales);

                var customerService = new AppClaimAdd();
                customerService.ClaimType = "OU";
                customerService.ClaimValue = "Customer Service";
                customerService.Description = "Assigned to employee accounts who work in the customer service department";
                m.AppClaimAdd(customerService);

                var marketing = new AppClaimAdd();
                marketing.ClaimType = "OU";
                marketing.ClaimValue = "Marketing";
                marketing.Description = "Assigned to employee accounts who work in the marketing department";
                m.AppClaimAdd(marketing);

                // Location OU's
                var toronto = new AppClaimAdd();
                toronto.ClaimType = "OU";
                toronto.ClaimValue = "Toronto";
                toronto.Description = "Assigned to accounts working at the Toronto office";
                m.AppClaimAdd(toronto);

                var london = new AppClaimAdd();
                london.ClaimType = "OU";
                london.ClaimValue = "London";
                london.Description = "Assigned to accounts working at the London office";
                m.AppClaimAdd(london);

                var newYork = new AppClaimAdd();
                newYork.ClaimType = "OU";
                newYork.ClaimValue = "New York";
                newYork.Description = "Assigned to accounts working at the New York office";
                m.AppClaimAdd(newYork);

                // Tasks
                var purchase = new AppClaimAdd();
                purchase.ClaimType = "Task";
                purchase.ClaimValue = "Purchase";
                purchase.Description = "Assigned to customers purchasing music or music products";
                m.AppClaimAdd(purchase);

                var setPrice = new AppClaimAdd();
                setPrice.ClaimType = "Task";
                setPrice.ClaimValue = "Set Price";
                setPrice.Description = "Assigned to sales employees who can set the price of music products";
                m.AppClaimAdd(setPrice);

                var modifyEmployeeInfo = new AppClaimAdd();
                modifyEmployeeInfo.ClaimType = "Task";
                modifyEmployeeInfo.ClaimValue = "Modify Employee";
                modifyEmployeeInfo.Description = "Assigned to employees who are authorized to make changes to other employees";
                m.AppClaimAdd(modifyEmployeeInfo);
            }
        }

    }
}
