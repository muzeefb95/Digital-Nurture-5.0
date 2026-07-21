using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFCore_RetailInventory
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("===============================================================================");
            Console.WriteLine("             MODULE 5: ENTITY FRAMEWORK CORE 8.0 HANDS-ON DEMO                ");
            Console.WriteLine("===============================================================================");

            using var context = new AppDbContext();
            await context.Database.EnsureCreatedAsync();

            // Lab 4: Inserting Initial Data into the Database
            Console.WriteLine("\n--- Lab 4: Inserting Initial Data ---");
            var electronics = new Category { Name = "Electronics" };
            var groceries = new Category { Name = "Groceries" };
            await context.Categories.AddRangeAsync(electronics, groceries);

            var product1 = new Product { Name = "Laptop", Price = 75000, Category = electronics, StockQuantity = 15 };
            var product2 = new Product { Name = "Rice Bag", Price = 1200, Category = groceries, StockQuantity = 40 };
            await context.Products.AddRangeAsync(product1, product2);
            await context.SaveChangesAsync();
            Console.WriteLine("  Initial Categories and Products added via AddRangeAsync & SaveChangesAsync.");

            // Lab 5: Retrieving Data from the Database
            Console.WriteLine("\n--- Lab 5: Retrieving Data ---");
            var allProducts = await context.Products.ToListAsync();
            Console.WriteLine("  All Products in Store:");
            foreach (var p in allProducts)
            {
                Console.WriteLine($"    [ID: {p.Id}] {p.Name,-15} | Price: ₹{p.Price,-8} | Stock: {p.StockQuantity}");
            }

            var foundProduct = await context.Products.FindAsync(1);
            Console.WriteLine($"  Find Product by ID(1): {foundProduct?.Name}");

            var expensiveProduct = await context.Products.FirstOrDefaultAsync(p => p.Price > 50000);
            Console.WriteLine($"  FirstOrDefault (Price > 50000): {expensiveProduct?.Name} (₹{expensiveProduct?.Price})");

            // Lab 6: Updating and Deleting Records
            Console.WriteLine("\n--- Lab 6: Updating and Deleting Records ---");
            var laptop = await context.Products.FirstOrDefaultAsync(p => p.Name == "Laptop");
            if (laptop != null)
            {
                laptop.Price = 70000;
                await context.SaveChangesAsync();
                Console.WriteLine($"  Updated Laptop Price to: ₹{laptop.Price}");
            }

            var riceBag = await context.Products.FirstOrDefaultAsync(p => p.Name == "Rice Bag");
            if (riceBag != null)
            {
                context.Products.Remove(riceBag);
                await context.SaveChangesAsync();
                Console.WriteLine("  Deleted 'Rice Bag' from inventory.");
            }

            // Lab 7: Writing Queries with LINQ
            Console.WriteLine("\n--- Lab 7: Writing Queries with LINQ & DTO Projection ---");
            var filtered = await context.Products
                .Where(p => p.Price > 1000)
                .OrderByDescending(p => p.Price)
                .ToListAsync();

            Console.WriteLine("  Filtered Products (Price > 1000, OrderByDescending):");
            foreach (var p in filtered)
            {
                Console.WriteLine($"    {p.Name} - ₹{p.Price}");
            }

            var productDTOs = await context.Products
                .Select(p => new ProductDTO
                {
                    Name = p.Name,
                    CategoryName = p.Category != null ? p.Category.Name : "N/A",
                    Price = p.Price
                })
                .ToListAsync();

            Console.WriteLine("  Projected ProductDTOs:");
            foreach (var dto in productDTOs)
            {
                Console.WriteLine($"    [DTO] {dto.Name} ({dto.CategoryName}) - ₹{dto.Price}");
            }

            // Lab 10: Eager and Explicit Loading
            Console.WriteLine("\n--- Lab 10: Eager and Explicit Loading ---");
            var eagerProducts = await context.Products
                .Include(p => p.Category)
                .ToListAsync();
            Console.WriteLine($"  Eager Loaded Category for Product '{eagerProducts.First().Name}': {eagerProducts.First().Category?.Name}");

            var singleProduct = await context.Products.FirstAsync();
            await context.Entry(singleProduct).Reference(p => p.Category).LoadAsync();
            Console.WriteLine($"  Explicitly Loaded Category for Product '{singleProduct.Name}': {singleProduct.Category?.Name}");

            // Lab 13: Query Caching and Tracking Behavior
            Console.WriteLine("\n--- Lab 13: AsNoTracking Optimization ---");
            var noTrackingProducts = await context.Products
                .AsNoTracking()
                .ToListAsync();
            Console.WriteLine($"  Retrieved {noTrackingProducts.Count} products with AsNoTracking().");

            // Lab 15: Handling Concurrency with RowVersion
            Console.WriteLine("\n--- Lab 15: Concurrency Conflict Handling ---");
            try
            {
                await context.SaveChangesAsync();
                Console.WriteLine("  Concurrency check passed. Saved successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("  Concurrency conflict detected and caught successfully.");
            }

            Console.WriteLine("===============================================================================\n");
        }
    }
}
