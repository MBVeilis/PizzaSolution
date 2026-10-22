using PizzaPlace.Models.Types;

namespace PizzaPlace.Models;

public record MenuItem(string Description, PizzaRecipeType PizzaType, double Price, bool SoldOut = false);
