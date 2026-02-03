namespace Web_App_VM_Management_System.Entities
{
    public class VendItem:BaseEntity
    {
        public Product Product { get; set; }
        public int VendingMachineId { get; set; }
        public InventoryItem InventoryItem { get; set; }
        public int InventoryItemId { get; set; }
        public int FloorNumber { get; set; }
        public int ItemCount { get; set; }
    }
}
