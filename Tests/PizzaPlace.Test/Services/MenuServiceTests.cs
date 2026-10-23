using PizzaPlace.Services;

namespace PizzaPlace.Test.Services;

[TestClass]
public class MenuServiceTests
{
    private readonly MenuService _menuService = new();

    [TestMethod]
    public void GetMenuBeforeLunch()
    {
        // Arrange
        var menuDate = DateTimeOffset.Parse("2026-10-23T10:59:00+00:00");

        // Act
        var result = _menuService.GetMenu(menuDate);

        // Assert
        Assert.AreEqual("Standard Menu", result.Title);
    }
}