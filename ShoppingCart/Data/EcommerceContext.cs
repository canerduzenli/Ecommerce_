using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ShoppingCart.Data
{
    public class EcommerceContext : DbContext
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options)
            : base(options)
        {
        }

        public DbSet<ShoppingCart.Models.Products> Products { get; set; } = default!;

        public DbSet<ShoppingCart.Models.Country> Countries { get; set; } = default!;

        public DbSet<ShoppingCart.Models.Cart> Carts { get; set; } = default!;

        public DbSet<ShoppingCart.Models.Order> Orders { get; set; } = default!;

    }
}