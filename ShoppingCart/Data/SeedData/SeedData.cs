using ShoppingCart.Models;
using ShoppingCart.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data.SeedData
{
    public static class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new EcommerceContext(serviceProvider.GetRequiredService<DbContextOptions<EcommerceContext>>());

            context.Database.EnsureDeleted();
            context.Database.Migrate();

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Products
                    {
                        ProductId = Guid.NewGuid(),
                        Name = "Apple MacBook Pro (M1, 2020)",
                        Description = "13-inch, Apple M1 chip with 8‑core CPU, 8GB RAM, 256GB SSD storage",
                        AvailableQuantity = 25,
                        PriceCAD = 1299.99m
                    },
                    new Products
                    {
                        ProductId = Guid.NewGuid(),
                        Name = "Sony WH-1000XM4 Wireless Headphones",
                        Description = "Industry leading noise cancelling over-ear headphones.",
                        AvailableQuantity = 50,
                        PriceCAD = 349.99m
                    },
                    new Products
                    {
                        ProductId = Guid.NewGuid(),
                        Name = "Samsung Galaxy S21 5G",
                        Description = "6.2-inch 120Hz display, 64MP camera, 128GB storage.",
                        AvailableQuantity = 30,
                        PriceCAD = 799.99m
                    },
                    new Products
                    {
                        ProductId = Guid.NewGuid(),
                        Name = "Dyson V11 Absolute Pro Vacuum",
                        Description = "High torque cleaner head, LCD screen, up to 60 minutes run time.",
                        AvailableQuantity = 20,
                        PriceCAD = 699.99m
                    },
                    new Products
                    {
                        ProductId = Guid.NewGuid(),
                        Name = "Bose SoundLink Mini Bluetooth Speaker II",
                        Description = "Deep, loud, and immersive sound, with a built-in speakerphone.",
                        AvailableQuantity = 40,
                        PriceCAD = 199.99m
                    },
                    new Products
                    {
                        ProductId = Guid.NewGuid(),
                        Name = "Fitbit Charge 4 Fitness Tracker",
                        Description = "Built-in GPS, 24/7 heart rate, sleep tracking, and up to 7 days of battery life.",
                        AvailableQuantity = 75,
                        PriceCAD = 149.99m
                    },
                    new Products
                    {
                        ProductId = Guid.NewGuid(),
                        Name = "Logitech G Pro X Gaming Headset",
                        Description = "DTS Headphone:X 2.0 surround sound, Blue VO!CE microphone technology.",
                        AvailableQuantity = 60,
                        PriceCAD = 129.99m
                    },
                    new Products
                    {
                        ProductId = Guid.NewGuid(),
                        Name = "Canon EOS M50 Mirrorless Camera",
                        Description = "24.1 Megapixel APS-C sensor, 4K video, Vari-angle touchscreen.",
                        AvailableQuantity = 10,
                        PriceCAD = 579.99m
                    }
                );

                await context.SaveChangesAsync();
            }

            if (!context.Countries.Any())
            {
                context.Countries.AddRange(
                    new Country
                    {
                        CountryName = "Canada",
                        ConversionRate = 1.0m,
                        TaxRate = 0.07,
                    },
                    new Country
                    {
                        CountryName = "USA",
                        ConversionRate = 0.80m,
                        TaxRate = 0.05,
                    },
                    new Country
                    {
                        CountryName = "UK",
                        ConversionRate = 1.25m,
                        TaxRate = 0.06,
                    },
                    new Country
                    {
                        CountryName = "Australia",
                        ConversionRate = 0.95m,
                        TaxRate = 0.10,
                    },
                    new Country
                    {
                        CountryName = "Japan",
                        ConversionRate = 0.011m,
                        TaxRate = 0.08,
                    },
                    new Country
                    {
                        CountryName = "Germany",
                        ConversionRate = 1.50m,
                        TaxRate = 0.19,
                    },
                    new Country
                    {
                        CountryName = "India",
                        ConversionRate = 0.017m,
                        TaxRate = 0.18,
                    },
                    new Country
                    {
                        CountryName = "China",
                        ConversionRate = 0.20m,
                        TaxRate = 0.13,
                    },
                    new Country
                    {
                        CountryName = "Brazil",
                        ConversionRate = 0.24m,
                        TaxRate = 0.17,
                    },
                    new Country
                    {
                        CountryName = "South Africa",
                        ConversionRate = 0.085m,
                        TaxRate = 0.15,
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
