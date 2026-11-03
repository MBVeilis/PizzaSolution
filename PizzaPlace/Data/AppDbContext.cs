using Microsoft.EntityFrameworkCore;
using PizzaPlace.Models;

namespace PizzaPlace.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Only use Models and not the DTO models and the enum models under the Types folder.
        public DbSet<Menu> Menus => Set<Menu>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();

        public DbSet<PizzaAmount> PizzaAmounts => Set<PizzaAmount>();
        public DbSet<PizzaOrder> PizzaOrders => Set<PizzaOrder>();
        public DbSet<RecipeEntity> Recipes => Set<RecipeEntity>();
        public DbSet<RecipeIngredientEntity> RecipeIngredients => Set<RecipeIngredientEntity>();
        public DbSet<StockEntity> Stocks => Set<StockEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Title)
                    .IsRequired();

                entity.Ignore(m => m.Items);
            });

            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Description)
                    .IsRequired();

                entity.HasOne<Menu>()
                    .WithMany()
                    .HasForeignKey(m => m.MenuId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PizzaOrder>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Ignore(o => o.RequestedOrder);
            });

            modelBuilder.Entity<PizzaAmount>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.HasOne<PizzaOrder>()
                    .WithMany()
                    .HasForeignKey(a => a.PizzaOrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RecipeEntity>(entity =>
            {
                entity.HasKey(recipe => recipe.Id);

                entity.HasIndex(recipe => recipe.RecipeType)
                    .IsUnique();

                entity.HasMany(recipe => recipe.Ingredients)
                    .WithOne(ingredient => ingredient.Recipe)
                    .HasForeignKey(ingredient => ingredient.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RecipeIngredientEntity>(entity =>
            {
                entity.HasKey(ingredient => ingredient.Id);
            });

            modelBuilder.Entity<StockEntity>(entity =>
            {
                entity.HasKey(stock => stock.Id);

                entity.HasIndex(stock => stock.StockType)
                    .IsUnique();
            });
        }
    }
}
