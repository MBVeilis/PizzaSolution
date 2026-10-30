using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Factories;

/// <summary>
/// Producing one line of pizza. 
/// Taking 7 minutes to setup - and then 5 minutes less for every subsequent pizza of the same recipe type to a minimum of 4 minutes.
/// </summary>
public class AssemblyLinePizzaOven(TimeProvider timeProvider) : PizzaOven(timeProvider)
{
    private const int AssemblyLineCapacity = 1;
    public const int SetupTimeMinutes = 7;
    public const int SubsequentPizzaTimeSavingsInMinutes = 5;
    public const int MinimumCookingTimeMinutes = 4;

    protected override int Capacity => AssemblyLineCapacity;

    protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
    {
        PizzaRecipeType? previousRecipeType = null;
        var previousPreparationTime = 0;

        foreach (var (recipe, orderGuid) in recipeOrders)
        {
            var preparationTime = recipe.CookingTimeMinutes;
            if (previousRecipeType == recipe.RecipeType)
            {
                preparationTime = Math.Max(
                    MinimumCookingTimeMinutes,
                    previousPreparationTime - SubsequentPizzaTimeSavingsInMinutes);
            }
            else
            {
                preparationTime += SetupTimeMinutes;
            }

            _pizzaQueue.Enqueue((MakePizza(recipe, preparationTime), orderGuid));
            previousRecipeType = recipe.RecipeType;
            previousPreparationTime = preparationTime;
        }
    }

    private Func<Task<Pizza?>> MakePizza(PizzaRecipeDto recipe, int preparationTime) => async () =>
    {
        await CookPizza(preparationTime);

        return GetPizza(recipe.RecipeType);
    };
}
