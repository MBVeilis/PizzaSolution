using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Models;
using PizzaPlace.Services;

namespace PizzaPlace.Controllers;

[Route("api/recipe")]
public class RecipeController(IRecipeService recipeService, PizzaOrder order) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPizzaRecipes()
    {
        try
        {
            return Ok(
                await recipeService.GetPizzaRecipes(order)
            );
        }
        catch (PizzaException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddPizzaRecipe([FromBody] ComparableList<PizzaRecipeDto> recipeDtos)
    {
        try
        {
            return Ok(
                await recipeService.AddPizzaRecipe(recipeDtos)
            );
        }
        catch (PizzaException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
