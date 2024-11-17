using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiTask.Models;
using WebApiTask.Repositories.Interface;

namespace WebApiTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColdDrinksController : ControllerBase
    {
        private IColdDrinks _coldDrinksRepo;
        public ColdDrinksController(IColdDrinks d)
        {
            _coldDrinksRepo = d;
        }

        // Get All
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var coldDrinksNames = _coldDrinksRepo.GetAllColdDrinksNames();
            if (coldDrinksNames == null || coldDrinksNames.Any() == false)
            {
                return NotFound("No cold drinks found.");
            }
            return Ok(coldDrinksNames);
        }

        // Get total price
        [HttpGet]
        [Route("TotalPrice")]
        public IActionResult TotalPrice()
        {
            decimal totalPrice = _coldDrinksRepo.GetTotalPriceOfAllColdDrinks();
            return Ok("Total price of all cold drinks: " + totalPrice);
        }

        // Insert
        [HttpPost]
        [Route("CreateNew")]
        public IActionResult CreateNew(ColdDrinks coldDrink)
        {
            var result = _coldDrinksRepo.InsertColdDrinks(coldDrink);
            if (result == 0)
            {
                return BadRequest("Invalid data.");
            }
            return Ok("Cold drink added, ID: " + result);
        }

        //Update Price
        [HttpPut]
        [Route("EditPrice")]
        public IActionResult EditPrice(string name, decimal newUnitPrice)
        {
            var result = _coldDrinksRepo.UpdateUnitPrice(name, newUnitPrice);
            if (result == 0)
            {
                return BadRequest("Product Not Found. Failed to update the price.");
            }
            return Ok("Price updated.");
        }

        // Delete by name
        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(string name)
        {
            var result = _coldDrinksRepo.DeleteColdDrinksByName(name);
            if (result == 0)
            {
                return BadRequest("Failed to delete the cold drink.");
            }
            return Ok("Cold drink deleted.");
        }

        // Delete by quantity
        [HttpDelete]
        [Route("DeleteByQuantity")]
        public IActionResult DeleteByQuantity(int quantity)
        {
            int deletedCount = _coldDrinksRepo.DeleteColdDrinksByQuantity(quantity);
            if (deletedCount == 0)
            {
                return BadRequest("No cold drinks were deleted.");
            }
            return Ok(deletedCount + " cold drinks deleted.");
        }
    }
}
