using PizzaPlace.Models.Types;

namespace PizzaPlace.Models;

public record MenuItem(Guid Id, Guid MenuId, string Description, PizzaRecipeType PizzaType, double Price, bool SoldOut = false)
{
    // Compatibility constructor so that existing applications and test code doesn't stop compiling.
    public MenuItem(string description, PizzaRecipeType pizzaType, double price, bool soldOut = false)
        : this(Guid.NewGuid(), Guid.Empty, description, pizzaType, price, soldOut)
    {
    }
}
