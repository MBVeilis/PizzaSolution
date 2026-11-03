using Microsoft.EntityFrameworkCore;
using PizzaPlace.Data;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;

namespace PizzaPlace.Repositories;

public class StockRepository(AppDbContext dbContext) : IStockRepository
{
    public async Task<StockDto> AddToStock(StockDto stock)
    {
        if (stock.Amount < 0)
            throw new PizzaException("Stock cannot have negative amount.");

        var stockEntity = await dbContext.Stocks
            .FirstOrDefaultAsync(existing => existing.StockType == stock.StockType);

        if (stockEntity is null)
        {
            stockEntity = new StockEntity
            {
                StockType = stock.StockType,
                Amount = stock.Amount
            };
            dbContext.Stocks.Add(stockEntity);
        }
        else
        {
            stockEntity.Amount += stock.Amount;
        }

        await dbContext.SaveChangesAsync();

        return new StockDto(stockEntity.StockType, stockEntity.Amount, stockEntity.Id);
    }

    public async Task<StockDto> GetStock(StockType stockType)
    {
        var stock = await dbContext.Stocks
            .AsNoTracking()
            .FirstOrDefaultAsync(existing => existing.StockType == stockType);

        return stock is null
            ? new StockDto(stockType, 0)
            : new StockDto(stock.StockType, stock.Amount, stock.Id);
    }

    public async Task<StockDto> TakeStock(StockType stockType, int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Unable to take zero or negative amount.");

        var stock = await dbContext.Stocks
            .FirstOrDefaultAsync(existing => existing.StockType == stockType);

        if (stock is null || stock.Amount < amount)
            throw new PizzaException("Not enough stock to take the given amount.");

        stock.Amount -= amount;
        await dbContext.SaveChangesAsync();

        return new StockDto(stockType, amount);
    }
}
