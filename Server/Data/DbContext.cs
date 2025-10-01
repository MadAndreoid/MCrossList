using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MCrossList.Server.Models.db;

namespace MCrossList.Server.Data
{
    public partial class dbContext : DbContext
    {
        public dbContext()
        {
        }

        public dbContext(DbContextOptions<dbContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MCrossList.Server.Models.db.Category>()
              .HasOne(i => i.Category_Father)
              .WithMany(i => i.InverseCategory_Father)
              .HasForeignKey(i => i.Category_Father_ID)
              .HasPrincipalKey(i => i.ID);

            builder.Entity<MCrossList.Server.Models.db.Product>()
              .HasOne(i => i.Product_Brand)
              .WithMany(i => i.Products)
              .HasForeignKey(i => i.Product_Brand_ID)
              .HasPrincipalKey(i => i.ID);

            builder.Entity<MCrossList.Server.Models.db.Product>()
              .HasOne(i => i.Product_Category)
              .WithMany(i => i.Products)
              .HasForeignKey(i => i.Product_Category_ID)
              .HasPrincipalKey(i => i.ID);

            builder.Entity<MCrossList.Server.Models.db.Product>()
              .HasOne(i => i.Product_Color)
              .WithMany(i => i.Products)
              .HasForeignKey(i => i.Product_Color_ID)
              .HasPrincipalKey(i => i.ID);

            builder.Entity<MCrossList.Server.Models.db.Product>()
              .HasOne(i => i.Product_Condition)
              .WithMany(i => i.Products)
              .HasForeignKey(i => i.Product_Condition_ID)
              .HasPrincipalKey(i => i.ID);

            builder.Entity<MCrossList.Server.Models.db.Product>()
              .HasOne(i => i.Product_Material)
              .WithMany(i => i.Products)
              .HasForeignKey(i => i.Product_Material_ID)
              .HasPrincipalKey(i => i.ID);

            builder.Entity<MCrossList.Server.Models.db.Product>()
              .HasOne(i => i.Product_Size)
              .WithMany(i => i.Products)
              .HasForeignKey(i => i.Product_Size_ID)
              .HasPrincipalKey(i => i.ID);

            builder.Entity<MCrossList.Server.Models.db.Product>()
              .HasOne(i => i.Product_Store)
              .WithMany(i => i.Products)
              .HasForeignKey(i => i.Product_Store_ID)
              .HasPrincipalKey(i => i.ID);
            this.OnModelBuilding(builder);
        }

        public DbSet<MCrossList.Server.Models.db.Brand> Brands { get; set; }

        public DbSet<MCrossList.Server.Models.db.Category> Categories { get; set; }

        public DbSet<MCrossList.Server.Models.db.Color> Colors { get; set; }

        public DbSet<MCrossList.Server.Models.db.Condition> Conditions { get; set; }

        public DbSet<MCrossList.Server.Models.db.Material> Materials { get; set; }

        public DbSet<MCrossList.Server.Models.db.Product> Products { get; set; }

        public DbSet<MCrossList.Server.Models.db.Size> Sizes { get; set; }

        public DbSet<MCrossList.Server.Models.db.Store> Stores { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}