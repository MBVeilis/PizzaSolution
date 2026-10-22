using Microsoft.AspNetCore.Mvc;

namespace PizzaPlace.Controllers;

[Route("api/welcome")]
public class WelcomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Greet()
    {
        Console.WriteLine("Greeted guest.");
        Console.WriteLine("The waiter greeted the guest.");

        return Ok("Welcome to this automated pizza place.");
    }
}
