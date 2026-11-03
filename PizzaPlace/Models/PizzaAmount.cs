using System.Text.Json.Serialization;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Models;

public record PizzaAmount
{
    public Guid Id { get; init; }
    public Guid PizzaOrderId { get; init; }
    public PizzaRecipeType PizzaType { get; init; }
    public ushort Amount { get; init; }

    [JsonConstructor]
    public PizzaAmount(Guid id, Guid pizzaOrderId, PizzaRecipeType pizzaType, ushort amount)
    {
        Id = id;
        PizzaOrderId = pizzaOrderId;
        PizzaType = pizzaType;
        Amount = amount;
    }

    public PizzaAmount()
        : this(Guid.Empty, Guid.Empty, default, 0)
    {
    }

    public PizzaAmount(PizzaRecipeType pizzaType, ushort amount)
        : this(Guid.NewGuid(), Guid.Empty, pizzaType, amount)
    {
    }
}