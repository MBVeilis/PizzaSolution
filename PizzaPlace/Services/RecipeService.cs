using PizzaPlace.Models;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services;

public class RecipeService(IRecipeRepository recipeRepository) : IRecipeService
{
    public async Task<ComparableList<PizzaRecipeDto>> GetPizzaRecipes(PizzaOrder order)
    {
        var pizzaTypes = order.RequestedOrder
            .Select(x => x.PizzaType)
            .Distinct()
            .ToList();

        ComparableList<PizzaRecipeDto> recipes = [];
        foreach (var pizzaType in pizzaTypes)
        {
            recipes.Add(await recipeRepository.GetRecipe(pizzaType));
        }

        return recipes;
    }

    public async Task<ComparableList<PizzaRecipeDto>> AddPizzaRecipe(ComparableList<PizzaRecipeDto> recipeDtos)
    {
        if (recipeDtos is null)
            throw new PizzaException("Can't add a recipe that doesn't exist");

        var recipes = new ComparableList<PizzaRecipeDto>();

        foreach (var recipe in recipeDtos)
        {
            var id = await recipeRepository.AddRecipe(recipe);
            recipes.Add(recipe with { Id = id });
        }

        return recipes;
    }
}
