using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services;

[TestClass]
public class StockServiceTests
{
    private static ComparableList<PizzaRecipeDto> GetRecipes() =>
    [
        new(
            PizzaRecipeType.StandardPizza,
            [
                new(StockType.Dough, 2),
                new(StockType.Tomatoes, 3)
            ],
            10)
    ];

    [TestMethod]
    public async Task HasInsufficientStock_ReturnsTrue_WhenIngredientIsMissing()
    {
        // Arrange
        var repository = new FakeStockRepository();
        var service = new StockService(repository);

        var recipes = GetRecipes();

        var order = new PizzaOrder(
            [
                new(PizzaRecipeType.StandardPizza, 1)
            ]);

        // Act
        var actual = await service.HasInsufficientStock(
            order,
            recipes);

        // Assert
        Assert.IsTrue(actual);
    }

    [TestMethod]
    public async Task HasInsufficientStock_ReturnsFalse_WhenStockIsSufficient()
    {
        // Arrange
        var repository = new FakeStockRepository();
        var service = new StockService(repository);

        var recipes = GetRecipes();

        await repository.AddToStock(
            new StockDto(StockType.Dough, 100));

        await repository.AddToStock(
            new StockDto(StockType.Tomatoes, 100));

        var order = new PizzaOrder(
            [
                new(PizzaRecipeType.StandardPizza, 1)
            ]);

        // Act
        var actual = await service.HasInsufficientStock(
            order,
            recipes);

        // Assert
        Assert.IsFalse(actual);

        // Cleanup
        var cleanUpDough = await repository.TakeStock(
            StockType.Dough, 100);

        var cleanUpTomatoes = await repository.TakeStock(
            StockType.Tomatoes, 100);

        // Assert
        Assert.AreEqual(cleanUpDough, new StockDto(StockType.Dough, 100));
        Assert.AreEqual(cleanUpTomatoes, new StockDto(StockType.Tomatoes, 100));
    }

    [TestMethod]
    public async Task HasInsufficientStock_AggregatesIngredientsAcrossPizzas()
    {
        // Arrange
        var repository = new FakeStockRepository();
        var service = new StockService(repository);

        var recipes = GetRecipes();

        await repository.AddToStock(
            new StockDto(StockType.Dough, 3));

        await repository.AddToStock(
            new StockDto(StockType.Tomatoes, 100));

        var order = new PizzaOrder(
            [
                new(PizzaRecipeType.StandardPizza, 2)
            ]);

        // Act
        var actual = await service.HasInsufficientStock(
            order,
            recipes);

        // Assert
        Assert.IsTrue(actual);

        // Cleanup
        var cleanUpDough = await repository.TakeStock(
            StockType.Dough, 3);

        var cleanUpTomatoes = await repository.TakeStock(
            StockType.Tomatoes, 100);

        // Assert
        Assert.AreEqual(cleanUpDough, new StockDto(StockType.Dough, 3));
        Assert.AreEqual(cleanUpTomatoes, new StockDto(StockType.Tomatoes, 100));
    }

    [TestMethod]
    public async Task GetStock_ReturnsStockAfterOrder()
    {
        // Arrange
        var repository = new FakeStockRepository();
        var service = new StockService(repository);

        var recipes = GetRecipes();

        await repository.AddToStock(
            new StockDto(StockType.Dough, 10));

        await repository.AddToStock(
            new StockDto(StockType.Tomatoes, 10));

        var order = new PizzaOrder(
            [
                new(PizzaRecipeType.StandardPizza, 2)
            ]);

        // Act
        var actual = await service.GetStock(order, recipes);

        // Assert
        Assert.AreEqual(
            6,
            actual.Single(x => x.StockType == StockType.Dough).Amount);

        Assert.AreEqual(
            4,
            actual.Single(x => x.StockType == StockType.Tomatoes).Amount);
    }
}
