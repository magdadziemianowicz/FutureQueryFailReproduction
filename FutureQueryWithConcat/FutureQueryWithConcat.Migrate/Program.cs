using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FutureQueryWithConcat.DbContext;
using FutureQueryWithConcat.Models;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace FutureQueryWithConcat.Migrate
{
    static class Program
    {
        static void Main(string[] args)
        {
            Task t = MainAsync(args);
            t.Wait();
        }
        static async Task<int> MainAsync(string[] args)
        {
            try
            {
                using (var context = new FutureDbContext())
                {
                    PrepareDatabase(context);

                    var namesList1 = new List<string>() {"Test2"};
                    var namesList2 = new List<string>() {"Test", "Test2"};

                    var counterIdsWithFuture = GetIdsForTenantsAndUsers(namesList1, context).Future();
                    var counterIdsWithFuture2 = GetIdsForTenantsAndUsers(namesList2, context).Future();

                    Console.WriteLine("Testing without future.");
                    var resultWithoutFuture = await GetIdsForTenantsAndUsers(namesList1, context).ToListAsync();
                    var resultWithoutFuture2 = await GetIdsForTenantsAndUsers(namesList2, context).ToListAsync();
                    Console.WriteLine($"Completed.");

                    Console.WriteLine("Testing with future.");
                    // Here fails
                    var resultWithFuture = await counterIdsWithFuture.ToListAsync();
                    var resultWithFuture2 = await counterIdsWithFuture2.ToListAsync();
                    Console.WriteLine("Completed.");

                    if (resultWithoutFuture == resultWithFuture && resultWithoutFuture2 == resultWithFuture2)
                    {
                        Console.WriteLine("Results are the same.");
                    }
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
                throw;
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void PrepareDatabase(FutureDbContext context)
        {
            Console.WriteLine("Preparing database.");
            context.Database.Migrate();
            context.Database.EnsureCreated();

            context.Tenants.RemoveRange(context.Tenants);
            context.Users.RemoveRange(context.Users);
            context.SaveChanges();

            for (int i = 0; i < 300; i++)
            {
                context.Tenants.Add(new Tenant() {Name = "Test"});
                context.Tenants.Add(new Tenant() {Name = "Test2"});
                context.Users.Add(new User() {Name = "Test"});
            }

            context.SaveChanges();
            Console.WriteLine("Database has been prepared.");
        }
        private static IQueryable<int> GetIdsForTenantsAndUsers(List<string> names, FutureDbContext context)
        {
            // Return all ids from tenants and users
            return context.Users
                .Where(u => names.Contains(u.Name))
                .Select(u => u.Id)
                .Concat(context.Tenants
                    .Where(t => names.Contains(t.Name))
                    .Select(t => t.Id))
                .OrderBy(userId => userId);
        }
    }
}
