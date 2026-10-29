using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Controllers;

[TestClass]
public class RecipeControllerTests
{
    private static RecipeController GetController(Mock<IRecipeService> recipeService, PizzaOrder order) =>
        new(recipeService.Object, order);

    [TestMethod]
    public async Task GetPizzaRecipesTest()
    {
        // Arrange
        var order = new PizzaOrder([
            new PizzaAmount(PizzaRecipeType.StandardPizza, 1)
        ]);
        ComparableList<PizzaRecipeDto> recipes = [
            new PizzaRecipeDto(PizzaRecipeType.StandardPizza, [], 10, 1)
        ];

        var recipeService = new Mock<IRecipeService>(MockBehavior.Strict);
        recipeService.Setup(x => x.GetPizzaRecipes(order))
            .ReturnsAsync(recipes);

        var controller = GetController(recipeService, order);

        // Act
        var actual = await controller.GetPizzaRecipes();

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        Assert.AreEqual(recipes, ((OkObjectResult)actual).Value);

        recipeService.VerifyAll();
    }

    [TestMethod]
    public async Task AddPizzaRecipeTest()
    {
        // Arrange
        ComparableList<PizzaRecipeDto> recipes = [
            new PizzaRecipeDto(PizzaRecipeType.ExtremelyTastyPizza, [], 15)
        ];

        var recipeService = new Mock<IRecipeService>(MockBehavior.Strict);
        recipeService.Setup(x => x.AddPizzaRecipe(recipes))
            .ReturnsAsync(recipes);

        var controller = GetController(recipeService, new PizzaOrder([]));

        // Act
        var actual = await controller.AddPizzaRecipe(recipes);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(actual);
        Assert.AreEqual(recipes, ((OkObjectResult)actual).Value);

        recipeService.VerifyAll();
    }
}
