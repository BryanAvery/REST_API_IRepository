using RepositoryRESTAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HttpClientSample
{
    internal class Program
    {
        private static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            var repository = new Repository("http://localhost:64195/api/");

            try
            {
                // Create a new product
                Product product = new Product
                {
                    Name = "Gizmo",
                    Price = 100,
                    Category = "Widgets"
                };

                var url = await repository.AddAsync(product, "products");

                // Get the product
                var products = await repository.GetAsync<List<Product>>("products");

                // Update the product
                Console.WriteLine("Updating price...");
                product.Price = 80;
                await repository.EditAsync(product, "products/1");

                // Get the updated product
                product = await repository.GetAsync<Product>("products/1");

                // Delete the product
                var statusCode = await repository.DeleteAsync($"products/{product.Id}");
                Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}