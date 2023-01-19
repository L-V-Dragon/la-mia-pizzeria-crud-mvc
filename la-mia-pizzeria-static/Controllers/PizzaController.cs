using Microsoft.AspNetCore.Mvc;
using la_mia_pizzeria_static.Models;
using Microsoft.Extensions.Hosting;
using la_mia_pizzeria_static.Database;
using Microsoft.SqlServer.Server;
using Microsoft.EntityFrameworkCore;

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
				}

			using (PizzaContext db = new PizzaContext())
            {
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
                Pizza pizzaToUpdate = db.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzaToUpdate == null)
                {
                    return NotFound("La pizza non è stata trovata");
                }

				List<Category> categories = db.Categories.ToList<Category>();

				PizzaCategoryView modelForView = new PizzaCategoryView();
				modelForView.Pizza = pizzaToUpdate;
				modelForView.Categories = categories;

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
                Pizza pizzaToUpdate = db.Pizze.Where(pizza => pizza.Id == formData.Pizza.Id).FirstOrDefault();

                if (pizzaToUpdate != null)
                {
                    pizzaToUpdate.Nome = formData.Pizza.Nome;
                    pizzaToUpdate.Foto = formData.Pizza.Foto;
                    pizzaToUpdate.Descrizione = formData.Pizza.Descrizione;
                    pizzaToUpdate.Toppings = formData.Pizza.Toppings;
                    pizzaToUpdate.Prezzo = formData.Pizza.Prezzo;
                    pizzaToUpdate.CategoryId = formData.Pizza.CategoryId;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("La pizza che volevi modificare non è stata trovata!");
                }
            }

        }

		[HttpGet]
		public IActionResult Delete(int id)
		{
			using (PizzaContext db = new PizzaContext())
			{
				Pizza pizzaToDelete = db.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();

				if (pizzaToDelete == null)
				{
					return NotFound("La pizza non è stata trovata");
				}

				List<Category> categories = db.Categories.ToList<Category>();

				PizzaCategoryView modelForView = new PizzaCategoryView();
				modelForView.Pizza = pizzaToDelete;
				modelForView.Categories = categories;

				return View("Delete", modelForView);
			}

		}

		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(PizzaCategoryView formData)
        {
			if (!ModelState.IsValid)
			{
				using (PizzaContext db = new PizzaContext())
				{
					List<Category> categories = db.Categories.ToList<Category>();

					formData.Categories = categories;
				}

				return View("Delete", formData);
			}

			using (PizzaContext db = new PizzaContext())
            {
				Pizza pizzaToDelete = db.Pizze.Where(pizza => pizza.Id == formData.Pizza.Id).FirstOrDefault();

				if (pizzaToDelete != null)
                {
					pizzaToDelete.Nome = formData.Pizza.Nome;
                    pizzaToDelete.Foto = formData.Pizza.Foto;
					pizzaToDelete.Descrizione = formData.Pizza.Descrizione;
                    pizzaToDelete.Toppings = formData.Pizza.Toppings;
					pizzaToDelete.Prezzo = formData.Pizza.Prezzo;
                    pizzaToDelete.CategoryId = formData.Pizza.CategoryId;

					db.SaveChanges();

					return RedirectToAction("Index");
				}
				else
				{
					return NotFound("La pizza che volevi eliminare non è stata trovata!");
				}
			}
        }

    }
}

