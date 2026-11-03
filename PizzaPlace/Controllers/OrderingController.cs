using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Services;

namespace PizzaPlace.Controllers;

[Route("api/order")]
[ApiController]
public class OrderingController(
    IOrderingService orderingService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PlacePizzaOrder([FromBody] PizzaOrder pizzaOrder)
    {
        if (pizzaOrder is null || pizzaOrder.RequestedOrder is null)
            return BadRequest("An order with at least an order collection is required.");

        return Ok(new
        {
            pizzas = await orderingService.HandlePizzaOrder(pizzaOrder),
        });
    }
}
