using Microsoft.AspNetCore.Mvc;
using la_mia_pizzeria_static.Models;
using Microsoft.Extensions.Hosting;
using la_mia_pizzeria_static.Database;
using Microsoft.SqlServer.Server;
using Microsoft.EntityFrameworkCore;
using la_mia_pizzeria_static.Utils;
using Azure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {
        public IActionResult Index()
        {
            using PizzaContext db = new PizzaContext();
            List<Pizza> listaDellePizze = db.Pizze.ToList();
            return View("Index", listaDellePizze);
        }

        public IActionResult Details(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {

                Pizza pizzaTrovata = db.Pizze
                    .Where(SingolaPizzaNelDb => SingolaPizzaNelDb.Id == id)
					.Include(pizza => pizza.Category)
                    .Include(pizza => pizza.Toppings)
                    .FirstOrDefault();


                if (pizzaTrovata != null)
                {
                    return View(pizzaTrovata);
                }

                return NotFound("La pizza con l'id cercato non esiste!");

            }
        }

        [HttpGet]
        public IActionResult Create()
        {
			using (PizzaContext db = new PizzaContext())
			{
				List<Category> categoriesFromDb = db.Categories.ToList<Category>();

				PizzaCategoryView modelForView = new PizzaCategoryView();
				modelForView.Pizza = new Pizza();

				modelForView.Categories = categoriesFromDb;
                modelForView.Toppings = ToppingConverter.getListToppingsForMultipleSelect();

                return View("Create", modelForView);
			}
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaCategoryView formData)
        {
            if (!ModelState.IsValid)
				using (PizzaContext db = new PizzaContext())
				{
					List<Category> categories = db.Categories.ToList<Category>();

					formData.Categories = categories;
                    formData.Toppings = ToppingConverter.getListToppingsForMultipleSelect();
                }

			using (PizzaContext db = new PizzaContext())
            {
                if (formData.ToppingsSelectedFromMultipleSelect != null)
                {
                    formData.Pizza.Toppings = new List<Topping>();

                    foreach (string tagId in formData.ToppingsSelectedFromMultipleSelect)
                    {
                        int tagIdIntFromSelect = int.Parse(tagId);

                        Topping topping = db.Toppings.Where(tagDb => tagDb.Id == tagIdIntFromSelect).FirstOrDefault();

                        
                        formData.Pizza.Toppings.Add(topping);
                    }
                }
                db.Pizze.Add(formData.Pizza);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaToUpdate = db.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Toppings).FirstOrDefault();

                if (pizzaToUpdate == null)
                {
                    return NotFound("La pizza non è stata trovata");
                }

				List<Category> categories = db.Categories.ToList<Category>();

				PizzaCategoryView modelForView = new PizzaCategoryView();
				modelForView.Pizza = pizzaToUpdate;
				modelForView.Categories = categories;

                List<Topping> listToppingFromDb = db.Toppings.ToList<Topping>();

                List<SelectListItem> listaOpzioniPerLaSelect = new List<SelectListItem>();

                foreach (Topping topping in listToppingFromDb)
                {
                    bool eraStatoSelezionato = pizzaToUpdate.Toppings.Any(toppingsSelezionati => toppingsSelezionati.Id == topping.Id);

                    SelectListItem opzioneSingolaSelect = new SelectListItem() { Text = topping.Name, Value = topping.Id.ToString(), Selected = eraStatoSelezionato };
                    listaOpzioniPerLaSelect.Add(opzioneSingolaSelect);
                }
                modelForView.Toppings = listaOpzioniPerLaSelect;

                return View("Update", modelForView);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(PizzaCategoryView formData)
        {
            if (!ModelState.IsValid)
            {
				using (PizzaContext db = new PizzaContext())
				{
					List<Category> categories = db.Categories.ToList<Category>();

					formData.Categories = categories;
				}

				return View("Update", formData);
            }

            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaToUpdate = db.Pizze.Where(pizza => pizza.Id == formData.Pizza.Id).Include(pizza => pizza.Toppings).FirstOrDefault();

                if (pizzaToUpdate != null)
                {
                    pizzaToUpdate.Nome = formData.Pizza.Nome;
                    pizzaToUpdate.Foto = formData.Pizza.Foto;
                    pizzaToUpdate.Descrizione = formData.Pizza.Descrizione;
                    pizzaToUpdate.Prezzo = formData.Pizza.Prezzo;
                    pizzaToUpdate.CategoryId = formData.Pizza.CategoryId;

                    pizzaToUpdate.Toppings.Clear();

                    if (formData.ToppingsSelectedFromMultipleSelect != null)
                    {

                        foreach (string tagId in formData.ToppingsSelectedFromMultipleSelect)
                        {
                            int tagIdIntFromSelect = int.Parse(tagId);

                            Topping topping = db.Toppings.Where(tagDb => tagDb.Id == tagIdIntFromSelect).FirstOrDefault();


                            pizzaToUpdate.Toppings.Add(topping);
                        }
                    }

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("La pizza che volevi modificare non è stata trovata!");
                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Delete(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaToDelete = db.Pizze.Where(post => post.Id == id).FirstOrDefault();

                if (pizzaToDelete != null)
                {
                    db.Pizze.Remove(pizzaToDelete);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("Il post da eliminare non è stato trovato!");
                }
            }
        }

    }
}

