using System.Text.Json.Serialization;

namespace PizzaPlace.Models;

public record PizzaOrder
{
    public Guid Id { get; init; }
    public ComparableList<PizzaAmount> RequestedOrder { get; init; }

    [JsonConstructor]
    public PizzaOrder(Guid id, ComparableList<PizzaAmount> requestedOrder)
    {
        Id = id;
        RequestedOrder = requestedOrder;
    }

    public PizzaOrder()
        : this(Guid.Empty, [])
    {
    }

    public PizzaOrder(ComparableList<PizzaAmount> requestedOrder)
        : this(Guid.NewGuid(), requestedOrder)
    {
    }
}