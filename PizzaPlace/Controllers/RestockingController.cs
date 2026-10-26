using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Services;

namespace PizzaPlace.Controllers;

[Route("api/restocking")]
public class RestockingController(IStockService stockService, PizzaOrder order, ComparableList<PizzaRecipeDto> recipeDtos) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Restock([FromBody] ComparableList<StockDto> stock)
    {
        try
        {
            return Ok(
                await stockService.Restock(stock)
            );
        }
        catch (PizzaException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetStock()
    {
        try
        {
            return Ok(
                await stockService.GetStock(order, recipeDtos)
            );
        }
        catch (PizzaException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
