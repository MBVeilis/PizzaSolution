using PizzaPlace.Models;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Factories;

/// <summary>
/// Produces pizza on one giant revolving surface. Downside is, that all pizzas must have the same cooking time, when prepared at the same time.
/// </summary>
public class GiantRevolvingPizzaOven(TimeProvider timeProvider) : PizzaOven(timeProvider)
{
    private const int GiantRevolvingPizzaOvenCapacity = 120;

    protected override int Capacity => GiantRevolvingPizzaOvenCapacity;

    protected override void PlanPizzaMaking(IEnumerable<(PizzaRecipeDto Recipe, Guid Guid)> recipeOrders)
    {
        var orders = recipeOrders.ToList();

        foreach (var cookingGroup in orders.GroupBy(order => order.Recipe.CookingTimeMinutes))
        {
            var groupOrders = cookingGroup.ToList();
            for (var start = 0; start < groupOrders.Count;)
            {
                var cookingTime = cookingGroup.Key;
                var end = Math.Min(start + Capacity, groupOrders.Count);

                for (var index = start; index < end; index++)
                {
                    var (recipe, orderGuid) = groupOrders[index];
                    _pizzaQueue.Enqueue((MakePizza(recipe, cookingTime), orderGuid));
                }

                for (var index = end - start; index < Capacity; index++)
                    _pizzaQueue.Enqueue((MakeEmptySpot(cookingTime), Guid.NewGuid()));

                start = end;
            }
        }
    }

    private Func<Task<Pizza?>> MakePizza(PizzaRecipeDto recipe, int cookingTime) => async () =>
    {
        await CookPizza(cookingTime);

        return GetPizza(recipe.RecipeType);
    };

    private Func<Task<Pizza?>> MakeEmptySpot(int cookingTime) => async () =>
    {
        await CookPizza(cookingTime);
        return null;
    };
}
