/*using Microsoft.EntityFrameworkCore;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
        await dbContext.Database.MigrateAsync();

        if (!await dbContext.Recipes.AnyAsync())
        {
            dbContext.Recipes.AddRange(
                new RecipeEntity
                {
                    RecipeType = PizzaRecipeType.StandardPizza,
                    CookingTimeMinutes = 10,
                    Ingredients =
                    [
                        new RecipeIngredientEntity { StockType = StockType.Dough, Amount = 1 },
                        new RecipeIngredientEntity { StockType = StockType.Tomatoes, Amount = 2 },
                        new RecipeIngredientEntity { StockType = StockType.GratedCheese, Amount = 1 },
                        new RecipeIngredientEntity { StockType = StockType.GenericSpices, Amount = 1 }
                    ]
                },
                new RecipeEntity
                {
                    RecipeType = PizzaRecipeType.ExtremelyTastyPizza,
                    CookingTimeMinutes = 10,
                    Ingredients =
                    [
                        new RecipeIngredientEntity { StockType = StockType.FermentedDough, Amount = 1 },
                        new RecipeIngredientEntity { StockType = StockType.Tomatoes, Amount = 1 },
                        new RecipeIngredientEntity { StockType = StockType.RottenTomatoes, Amount = 1 },
                        new RecipeIngredientEntity { StockType = StockType.GratedCheese, Amount = 1 },
                        new RecipeIngredientEntity { StockType = StockType.UngenericSpices, Amount = 1 }
                    ]
                });
        }

        if (!await dbContext.Stocks.AnyAsync())
        {
            dbContext.Stocks.AddRange(Enum.GetValues<StockType>()
                .Select(stockType => new StockEntity
                {
                    StockType = stockType,
                    Amount = 1000
                }));
        }

        await dbContext.SaveChangesAsync();
    }
}
*/