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
        var menuDate = DateTimeOffset.Parse("2026-10-26T10:59:00+00:00");

        // Act
        var result = _menuService.GetMenu(menuDate);

        // Assert
        Assert.AreEqual("Standard Menu", result.Title);
        Assert.AreNotEqual("Lunch Menu", result.Title);
    }

    [TestMethod]
    public void GetMenuAtLunch()
    {
        // Arrange
        var menuDate = DateTimeOffset.Parse("2026-10-26T11:00:00+00:00");

        // Act
        var result = _menuService.GetMenu(menuDate);

        // Assert
        Assert.AreEqual("Lunch Menu", result.Title);
        Assert.AreNotEqual("Standard Menu", result.Title);
    }

    [TestMethod]
    public void GetMenuAfterLunch()
    {
        // Arrange
        var menuDate = DateTimeOffset.Parse("2026-10-26T14:01:00+00:00");

        // Act
        var result = _menuService.GetMenu(menuDate);

        // Assert
        Assert.AreEqual("Standard Menu", result.Title);
        Assert.AreNotEqual("Lunch Menu", result.Title);
    }
}