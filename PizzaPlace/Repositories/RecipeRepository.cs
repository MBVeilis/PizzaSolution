using Microsoft.EntityFrameworkCore;
using PizzaPlace.Data;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories;

public class RecipeRepository(AppDbContext dbContext) : IRecipeRepository
{
    public async Task<long> AddRecipe(PizzaRecipeDto recipe)
    {
        var alreadyExists = await dbContext.Recipes
            .AnyAsync(existing => existing.RecipeType == recipe.RecipeType);

        if (alreadyExists)
            throw new PizzaException($"Recipe already added for {recipe.RecipeType}.");

        var recipeEntity = new RecipeEntity
        {
            RecipeType = recipe.RecipeType,
            CookingTimeMinutes = recipe.CookingTimeMinutes,
            Ingredients = recipe.Ingredients
                .Select(ingredient => new RecipeIngredientEntity
                {
                    StockType = ingredient.StockType,
                    Amount = ingredient.Amount
                })
                .ToList()
        };

        dbContext.Recipes.Add(recipeEntity);
        await dbContext.SaveChangesAsync();

        return recipeEntity.Id;
    }

    public async Task<PizzaRecipeDto> GetRecipe(PizzaRecipeType recipeType)
    {
        var recipe = await dbContext.Recipes
            .Include(entity => entity.Ingredients)
            .FirstOrDefaultAsync(entity => entity.RecipeType == recipeType)
            ?? throw new PizzaException($"Recipe does not exists of type {recipeType}.");

        return new PizzaRecipeDto(
            recipe.RecipeType,
            new ComparableList<StockDto>(recipe.Ingredients.Select(ingredient =>
                new StockDto(ingredient.StockType, ingredient.Amount, ingredient.Id))),
            recipe.CookingTimeMinutes,
            recipe.Id);
    }
}
