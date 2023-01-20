using Azure;
using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace la_mia_pizzeria_static.Utils
{
    public class ToppingConverter
    {
        public static List<SelectListItem> getListToppingsForMultipleSelect()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Topping> toppingsFromDb = db.Toppings.ToList<Topping>();

                
                List<SelectListItem> listaPerLaSelectMultipla = new List<SelectListItem>();

                foreach (Topping topping in toppingsFromDb)
                {
                    SelectListItem opzioneSingolaSelectMultipla = new SelectListItem() { Text = topping.Name, Value = topping.Id.ToString() };
                    listaPerLaSelectMultipla.Add(opzioneSingolaSelectMultipla);
                }

                return listaPerLaSelectMultipla;
            }
        }
    }
}

