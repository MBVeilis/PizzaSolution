using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;

namespace PizzaPlace.Services;

public class StockService(FakeStockRepository fakeStockRepository) : IStockService // Will be changed to IStockRepository when a proper Database has been implemeneted.
{
    private static Dictionary<StockType, int> GetRequiredIngredients(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        var required = new Dictionary<StockType, int>();

        foreach (var pizza in order.RequestedOrder)
        {
            var recipe = recipeDtos.FirstOrDefault(r => r.RecipeType == pizza.PizzaType);

            if (recipe is null)
            {
                throw new PizzaException(
                    $"No recipe found for {pizza.PizzaType}.");
            }

            foreach (var ingredient in recipe.Ingredients)
            {
                var amount = ingredient.Amount * pizza.Amount;

                required.TryGetValue(ingredient.StockType, out var current);
                required[ingredient.StockType] = current + amount;
            }
        }

        return required;
    }

    public async Task<bool> HasInsufficientStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        var required = GetRequiredIngredients(order, recipeDtos);

        foreach (var (stockType, amount) in required)
        {
            var stock = await fakeStockRepository.GetStock(stockType);

            if (stock.Amount < amount)
                return true;
        }

        return false;
    }

    public async Task<ComparableList<StockDto>> GetStock(PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos)
    {
        var required = GetRequiredIngredients(order, recipeDtos);
        var result = new ComparableList<StockDto>();

        foreach (var stockType in Enum.GetValues<StockType>())
        {
            var stock = await fakeStockRepository.GetStock(stockType);
            required.TryGetValue(stockType, out var amountRequired);

            result.Add(stock with
            {
                Amount = stock.Amount - amountRequired
            });
        }

        return result;
    }

    public async Task<ComparableList<StockDto>> Restock(ComparableList<StockDto> stock)
    {
        if (stock is null || stock.Any(s => s.Amount < 0))
            throw new PizzaException("Stock amounts cannot be negative.");

        var updated = new ComparableList<StockDto>();

        foreach (var item in stock)
            updated.Add(await fakeStockRepository.AddToStock(item));

        return updated;
    }
}
