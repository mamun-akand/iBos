using WebApiTask.Data;
using WebApiTask.Models;
using WebApiTask.Repositories.Interface;

namespace WebApiTask.Repositories.Implementations
{
    public class ColdDrinksRepository : IColdDrinks
    {
        private readonly ColdDrinksDBContext _context;
        public ColdDrinksRepository(ColdDrinksDBContext db)
        {
            _context = db;
        }

        //insert Cold Drinks
        public int InsertColdDrinks(ColdDrinks cold_drink)
        {
            if (cold_drink == null)
            {
                return 0;
            }
            
            _context.tblColdDrinks.Add(cold_drink);
            _context.SaveChanges();

            return (int)cold_drink.intColdDrinksId; 
        }

        //update Unit Price
        public int UpdateUnitPrice(string name, decimal newUnitPrice)
        {
            var drink = _context.tblColdDrinks.FirstOrDefault(d => d.strColdDrinksName == name);
            if (drink != null)
            {
                drink.numUnitPrice = newUnitPrice;
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }

        //delete Cold Drinks
        public int DeleteColdDrinksByName(string name)
        {
            var drink = _context.tblColdDrinks.FirstOrDefault(d => d.strColdDrinksName == name);
            if (drink != null)
            {
                _context.tblColdDrinks.Remove(drink);
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }

        //Get Remaining Products Name
        public List<string> GetAllColdDrinksNames()
        {
            return _context.tblColdDrinks.Select(d => d.strColdDrinksName).ToList();
        }

        //Delete products, If Quantity less than 
        public int DeleteColdDrinksByQuantity(int quantity)
        {
            var drinksToDelete = _context.tblColdDrinks.Where(d => d.numQuantity < quantity).ToList();

            int cnt = drinksToDelete.Count;
            if (cnt != 0)
            {
                _context.tblColdDrinks.RemoveRange(drinksToDelete);
                _context.SaveChanges();
                return cnt;
            }

            return 0;
            //int deletedCount = 0;
            //foreach (var drink in drinksToDelete)
            //{
            //    _context.tblColdDrinks.Remove(drink);
            //    deletedCount++;
            //}
            //_context.SaveChanges();

            //return deletedCount;
        }

        //Total Price of All Products
        public decimal GetTotalPriceOfAllColdDrinks()
        {
            //return _context.ColdDrinks.Sum(d => d.numQuantity * d.numUnitPrice);
            var allProductsSum = _context.tblColdDrinks.Select(d => d.numUnitPrice*d.numQuantity).Sum();           
            return allProductsSum;
        }
    }
}
