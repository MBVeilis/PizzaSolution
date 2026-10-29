using PizzaPlace.Models;

namespace PizzaPlace.Services;

public interface IRecipeService
{
    Task<ComparableList<PizzaRecipeDto>> GetPizzaRecipes(PizzaOrder order);
    Task<ComparableList<PizzaRecipeDto>> AddPizzaRecipe(ComparableList<PizzaRecipeDto> pizzaRecipe);
}
