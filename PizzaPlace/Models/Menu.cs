namespace PizzaPlace.Models;

public record Menu(Guid Id, string Title, ComparableList<MenuItem> Items)
{
    // Compatibility constructor so that existing applications and test code doesn't stop compiling.
    private Menu()
        : this(Guid.Empty, string.Empty, [])
    {
    }

    public Menu(string title, ComparableList<MenuItem> items)
        : this(Guid.NewGuid(), title, items)
    {
    }
}
