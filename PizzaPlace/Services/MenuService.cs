using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Pizzas;

namespace PizzaPlace.Services;

public class MenuService : IMenuService
{
    public Menu GetMenu(DateTimeOffset menuDate)
    {
        // Standard menu
        var standardMenu = StandardMenu(
            Title: "Standard Menu",
            Items: []
        );

        // Lunch menu
        var lunchMenu = LunchMenu(
            Title: "Lunch Menu",
            Items: []
        );

        // menuDate TBD
        // If else sentence

        return standardMenu;
    }

    private static Menu StandardMenu(string Title, List<MenuItem> Items)
    {
        if (Items == null)
            Items = [];

        Items.Add(new MenuItem("Pizza_pepperoni", PizzaRecipeType.StandardPizza, 99, false));
        Items.Add(new MenuItem("Pizza_gorgonzola", PizzaRecipeType.StandardPizza, 102, false));
        Items.Add(new MenuItem("Pizza_Vesuvio", PizzaRecipeType.StandardPizza, 94, false));
        Items.Add(new MenuItem("Pizza_Cappriciosa", PizzaRecipeType.StandardPizza, 98, false));
        Items.Add(new MenuItem("Pizza_Calzone", PizzaRecipeType.StandardPizza, 99, false));
        Items.Add(new MenuItem("Pizza_Quattro_Staggioni", PizzaRecipeType.StandardPizza, 105, false));
        Items.Add(new MenuItem("Pizza_Vegetariana", PizzaRecipeType.StandardPizza, 102, false));
        Items.Add(new MenuItem("Pizza_Bolognese", PizzaRecipeType.StandardPizza, 102, false));
        Items.Add(new MenuItem("Pizza_Shawarma", PizzaRecipeType.StandardPizza, 105, false));
        Items.Add(new MenuItem("Pizza_Diavolo", PizzaRecipeType.StandardPizza, 105, false));
        Items.Add(new MenuItem("Pizza_Vichinga", PizzaRecipeType.StandardPizza, 104, false));
        Items.Add(new MenuItem("Pizza_Napoletana", PizzaRecipeType.StandardPizza, 100, false));

        return new Menu(Title, [.. Items]);
    }

    private static Menu LunchMenu(string Title, List<MenuItem> Items)
    {
        if (Items == null)
            Items = [];

        Items.Add(new MenuItem("Pizza_margherita", PizzaRecipeType.StandardPizza, 87, false));
        Items.Add(new MenuItem("Pizza_marinara", PizzaRecipeType.StandardPizza, 105, false));
        Items.Add(new MenuItem("Pizza_Hawaii", PizzaRecipeType.StandardPizza, 98, false));
        Items.Add(new MenuItem("Pizza_Pepo_Italiano", PizzaRecipeType.StandardPizza, 107, false));
        Items.Add(new MenuItem("Pizza_Polpette_con_Pecorino", PizzaRecipeType.StandardPizza, 107, false));
        Items.Add(new MenuItem("Pizza_Romana", PizzaRecipeType.StandardPizza, 105, false));
        Items.Add(new MenuItem("Pizza_Sole", PizzaRecipeType.StandardPizza, 102, false));
        Items.Add(new MenuItem("Pizza_Costa_Smeralda", PizzaRecipeType.StandardPizza, 105, false));
        Items.Add(new MenuItem("Pizza_La_Salami", PizzaRecipeType.StandardPizza, 99, false));
        Items.Add(new MenuItem("Pizza_Rocco", PizzaRecipeType.StandardPizza, 103, false));
        Items.Add(new MenuItem("Pizza_Marco", PizzaRecipeType.StandardPizza, 105, false));
        Items.Add(new MenuItem("Pizza_Kokkodo", PizzaRecipeType.StandardPizza, 105, false));

        return new Menu(Title, [.. Items]);
    }
}
 