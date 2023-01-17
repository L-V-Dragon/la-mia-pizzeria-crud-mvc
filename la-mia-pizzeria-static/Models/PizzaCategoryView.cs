using Microsoft.Extensions.Hosting;

namespace la_mia_pizzeria_static.Models
{
	public class PizzaCategoryView
	{
		
		public Pizza Pizza { get; set; }

		public List<Category>? Categories { get; set; }
	}
}
