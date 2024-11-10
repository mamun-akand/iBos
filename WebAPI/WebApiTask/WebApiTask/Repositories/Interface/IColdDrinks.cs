using WebApiTask.Models;

namespace WebApiTask.Repositories.Interface
{
    public interface IColdDrinks 
    {
        int InsertColdDrinks(ColdDrinks cold_drinks); //done

        int UpdateUnitPrice(string name, decimal new_unit_price);

        int DeleteColdDrinksByName(string name);

        List<string> GetAllColdDrinksNames(); 

        int DeleteColdDrinksByQuantity(int quantity);

        decimal GetTotalPriceOfAllColdDrinks();
    }
}
