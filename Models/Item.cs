namespace Zone.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<IteminStore> Stores { get; set; } = new HashSet<IteminStore>();
       public ICollection<Invoice> Invioces { get; set; } = new HashSet<Invoice>();
    }
}
