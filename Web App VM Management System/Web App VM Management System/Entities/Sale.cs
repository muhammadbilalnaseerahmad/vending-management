using Microsoft.Identity.Client;

namespace Web_App_VM_Management_System.Entities
{
    public class Sale : BaseEntity
    {
        public InventoryItem InventoryItem { get; set; }
        public int ItemId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int TotalSale { get; set; }
        public int UnitSold { get; set; }
    }
}
