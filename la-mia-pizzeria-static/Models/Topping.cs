using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
	public class Topping
	{
		public int Id { get; set; }

		[Column(TypeName = "varchar(100)")]
		[StringLength(100, ErrorMessage = "Il campo titolo non può contenere più di 100 caratteri")]
		public string Name { get; set; }

		public List<Pizza> Pizze { get; set; }


		public Topping()
		{

		}
	}

	
}
