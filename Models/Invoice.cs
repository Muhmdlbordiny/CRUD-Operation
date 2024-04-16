using System.ComponentModel.DataAnnotations.Schema;

namespace Zone.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public Store Store { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
