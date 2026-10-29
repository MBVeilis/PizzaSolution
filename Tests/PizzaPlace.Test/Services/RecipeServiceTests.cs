using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Services;

[TestClass]
public class RecipeServiceTests
{
    private static RecipeService GetService(Mock<IRecipeRepository> recipeRepository) =>
        new(recipeRepository.Object);

    [TestMethod]
    public async Task GetPizzaRecipesTest()
    {
        // Arrange
        var order = new PizzaOrder([
            new PizzaAmount(PizzaRecipeType.RarePizza, 1),
            new PizzaAmount(PizzaRecipeType.OddPizza, 2),
            new PizzaAmount(PizzaRecipeType.RarePizza, 20),
        ]);
        var rareRecipe = new PizzaRecipeDto(PizzaRecipeType.RarePizza, [new StockDto(StockType.UnicornDust, 1)], 1);
        var oddRecipe = new PizzaRecipeDto(PizzaRecipeType.OddPizza, [new StockDto(StockType.Sulphur, 10)], 100);
        ComparableList<PizzaRecipeDto> expected = [rareRecipe, oddRecipe];

        var recipeRepository = new Mock<IRecipeRepository>(MockBehavior.Strict);
        recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.RarePizza))
            .ReturnsAsync(rareRecipe);
        recipeRepository.Setup(x => x.GetRecipe(PizzaRecipeType.OddPizza))
            .ReturnsAsync(oddRecipe);

        var service = GetService(recipeRepository);

        // Act
        var actual = await service.GetPizzaRecipes(order);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task AddPizzaRecipeTest()
    {
        // Arrange
        ComparableList<PizzaRecipeDto> recipes = [
            new PizzaRecipeDto(PizzaRecipeType.RarePizza, [new StockDto(StockType.UnicornDust, 1)], 1),
            new PizzaRecipeDto(PizzaRecipeType.OddPizza, [new StockDto(StockType.Sulphur, 10)], 100)
        ];
        ComparableList<PizzaRecipeDto> expected = [
            recipes[0] with { Id = 101 },
            recipes[1] with { Id = 102 }
        ];

        var recipeRepository = new Mock<IRecipeRepository>(MockBehavior.Strict);
        recipeRepository.Setup(x => x.AddRecipe(recipes[0]))
            .ReturnsAsync(101);
        recipeRepository.Setup(x => x.AddRecipe(recipes[1]))
            .ReturnsAsync(102);

        var service = GetService(recipeRepository);

        // Act
        var actual = await service.AddPizzaRecipe(recipes);

        // Assert
        Assert.AreEqual(expected, actual);

        recipeRepository.VerifyAll();
    }
}
