using PizzaPlace.Models.Types;

namespace PizzaPlace.Data;

public class StockEntity
{
    public long Id { get; set; }
    public StockType StockType { get; set; }
    public int Amount { get; set; }
}
