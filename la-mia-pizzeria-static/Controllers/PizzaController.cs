using Microsoft.AspNetCore.Mvc;
using la_mia_pizzeria_static.Models;
using Microsoft.Extensions.Hosting;
using la_mia_pizzeria_static.Database;
using Microsoft.SqlServer.Server;

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
                    .Where(SingolaPizzaNelDb => SingolaPizzaNelDb.ID == id)
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
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza formData)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", formData);
            }

            using (PizzaContext db = new PizzaContext())
            {
                db.Pizze.Add(formData);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaToUpdate = db.Pizze.Where(pizza => pizza.ID == id).FirstOrDefault();

                if (pizzaToUpdate == null)
                {
                    return NotFound("La pizza non è stata trovata");
                }

                return View("Update", pizzaToUpdate);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Pizza formData)
        {
            if (!ModelState.IsValid)
            {
                return View("Update", formData);
            }

            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaToUpdate = db.Pizze.Where(pizza => pizza.ID == formData.ID).FirstOrDefault();

                if (pizzaToUpdate != null)
                {
                    pizzaToUpdate.Nome = formData.Nome;
                    pizzaToUpdate.Foto = formData.Foto;
                    pizzaToUpdate.Descrizione = formData.Descrizione;
                    pizzaToUpdate.Prezzo = formData.Prezzo;

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
				Pizza pizzaToDelete = db.Pizze.Where(pizza => pizza.ID == id).FirstOrDefault();

				if (pizzaToDelete == null)
				{
					return NotFound("La pizza non è stata trovata");
				}

				return View("Delete", pizzaToDelete);
			}

		}

		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Pizza formData)
        {
			if (!ModelState.IsValid)
			{
				return View("Delete", formData);
			}

			using (PizzaContext db = new PizzaContext())
            {
				Pizza pizzaToDelete = db.Pizze.Where(pizza => pizza.ID == formData.ID).FirstOrDefault();

				if (pizzaToDelete != null)
                {
					pizzaToDelete.Nome = formData.Nome;
                    pizzaToDelete.Foto = formData.Foto;
					pizzaToDelete.Descrizione = formData.Descrizione;
					pizzaToDelete.Prezzo = formData.Prezzo;

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

