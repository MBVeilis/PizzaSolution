using PizzaPlace.Models.Types;

namespace PizzaPlace.Data;

public class RecipeEntity
{
    public long Id { get; set; }
    public PizzaRecipeType RecipeType { get; set; }
    public int CookingTimeMinutes { get; set; }
    public List<RecipeIngredientEntity> Ingredients { get; set; } = [];
}

public class RecipeIngredientEntity
{
    public long Id { get; set; }
    public long RecipeId { get; set; }
    public StockType StockType { get; set; }
    public int Amount { get; set; }

    public RecipeEntity Recipe { get; set; } = null!;
}
