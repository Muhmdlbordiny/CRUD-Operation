using System.ComponentModel.DataAnnotations;

namespace Zone.Models
{
    public class Store
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }

        public ICollection<IteminStore> Items { get; set; } = new HashSet<IteminStore>();
        public ICollection<Invoice> Invioces { get; set; } = new HashSet<Invoice>();
    }
}
