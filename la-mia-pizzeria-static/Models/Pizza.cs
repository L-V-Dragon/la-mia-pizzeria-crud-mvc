using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    public class Pizza
    {
        public int Id { get; set; }

		[Column(TypeName = "varchar(100)")]
        [StringLength(100, ErrorMessage = "Il campo titolo non può contenere più di 100 caratteri")]
        public string Nome{ get; set; }

		[Column(TypeName = "varchar(300)")]
        [StringLength(300, ErrorMessage = "Il campo titolo non può contenere più di 300 caratteri")]
        public string Foto { get; set; }

		[Column(TypeName = "text")]
		public string Descrizione { get; set; }

		[Column(TypeName = "varchar(10)")]
        [StringLength(10, ErrorMessage = "Il campo titolo non può contenere più di 10 caratteri")]
        public string Prezzo { get; set; }

		public int? CategoryId { get; set; }
		public Category? Category { get; set; }

		public List<Topping> Toppings { get; set; }

		public Pizza()
        {

        }

        public Pizza(int id, string nome, string foto, string descrizione, string prezzo)
        {
            Id = id; 
            Nome = nome;
            Foto = foto;
            Descrizione = descrizione;
            Prezzo = prezzo;
        }
    }
}
