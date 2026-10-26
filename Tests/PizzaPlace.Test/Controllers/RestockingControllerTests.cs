using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Controllers;
using PizzaPlace.Models;
using PizzaPlace.Models.Types;
using PizzaPlace.Services;

namespace PizzaPlace.Test.Controllers
{
    [TestClass]
    public class RestockingControllerTests
    {
        private static RestockingController GetController(Mock<IStockService> stockService)
        {
            var order = new PizzaOrder([]);
            var recipes = new ComparableList<PizzaRecipeDto>();

            return new RestockingController(
                stockService.Object,
                order,
                recipes);
        }

        [TestMethod]
        public async Task HandleRestock_Success()
        {
            // Arrange
            var stock = new ComparableList<StockDto>
            {
                new(StockType.Dough, 10)
            };

            var stockService = new Mock<IStockService>(
                MockBehavior.Strict);

            stockService.Setup(x => x.Restock(stock))
                .ReturnsAsync(stock);

            var controller = GetController(stockService);

            // Act
            var actual = await controller.Restock(stock);

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(actual);
            stockService.VerifyAll();
        }

        [TestMethod]
        public async Task HandleRestockIsNegative_ReturnBadRequest()
        {
            // Arrange
            var stock = new ComparableList<StockDto>
            {
                new(StockType.Dough, -1)
            };

            var stockService = new Mock<IStockService>(
                MockBehavior.Strict);

            stockService.Setup(x => x.Restock(stock))
                .ThrowsAsync(
                    new PizzaException("Stock amounts cannot be negative."));

            var controller = GetController(stockService);

            // Act
            var actual = await controller.Restock(stock);

            // Assert
            Assert.IsInstanceOfType<BadRequestObjectResult>(actual);
            stockService.VerifyAll();
        }

        [TestMethod]
        public async Task GetStock_ReturnsOk()
        {
            // Arrange
            var expected = new ComparableList<StockDto>
            {
                new(StockType.Dough, 5)
            };

            var stockService = new Mock<IStockService>(
                MockBehavior.Strict);

            stockService.Setup(x => x.GetStock(
                    It.IsAny<PizzaOrder>(),
                    It.IsAny<ComparableList<PizzaRecipeDto>>()))
                .ReturnsAsync(expected);

            var controller = GetController(stockService);

            // Act
            var actual = await controller.GetStock();

            // Assert
            Assert.IsInstanceOfType<OkObjectResult>(actual);

            var result = (OkObjectResult)actual;
            Assert.AreEqual(expected, result.Value);

            stockService.VerifyAll();
        }
    }
}
