using Microsoft.Extensions.Hosting;

namespace la_mia_pizzeria_static.Models
{
	public class Category
	{
		public int CategoryId { get; set; }
		public string Titolo { get; set; }

		public List<Pizza> Pizze { get; set; }

		public Category()
		{

		}
	}
}
