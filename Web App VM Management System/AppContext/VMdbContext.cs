using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;
using Web_App_VM_Management_System.Entities;

namespace Web_App_VM_Management_System.AppContext
{
    public class VMdbContext : IdentityDbContext<IdentityUser>
    {
        public VMdbContext(DbContextOptions<VMdbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Employee> Employees { get; set; } // Add this DbSet for employee salaries
        public DbSet<ItemCategory> ItemCategories { get; set; } // Add this DbSet for employee salaries
        public DbSet<InventoryItem> InventoryItems { get; set; } // Add this DbSet for employee salaries
        public DbSet<VendItem> VendItems { get; set; }
        public DbSet<Sale> Sale{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //fluent API
          //  modelBuilder.Entity<Category>()
          //.HasMany(c => c.Products)
          //.WithOne(p => p.ProductCategory)
          //.HasForeignKey(p => p.Catagoryid);

         //   modelBuilder.Entity<ItemCategory>()
         //.HasMany(c => c.InventoryItems)
         //.WithOne(p => p.ItemCategory)
         //.HasForeignKey(p => p.ItemCatagoryId);


            modelBuilder.Entity<VendItem>()
               .HasOne(pcp => pcp.Product)
               .WithMany(p => p.VendItems)
               .HasForeignKey(pcp => pcp.VendingMachineId);



            modelBuilder.Entity<VendItem>()
                   .HasOne(pcp => pcp.InventoryItem)
                   .WithMany(p => p.VendItems)
                   .HasForeignKey(pcp => pcp.InventoryItemId);



            modelBuilder.Entity<Sale>()
                   .HasOne(pcp => pcp.InventoryItem)
                   .WithMany(p => p.Sales)
                   .HasForeignKey(pcp => pcp.ItemId);


            modelBuilder.Entity<Sale>()
                   .HasOne(pcp => pcp.Product)
                   .WithMany(p => p.Sales)
                   .HasForeignKey(pcp => pcp.ProductId);



        }
    }
}
