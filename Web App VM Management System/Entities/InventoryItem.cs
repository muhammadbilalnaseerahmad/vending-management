namespace Web_App_VM_Management_System.Entities
{
    public class InventoryItem:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int QuantityInStock { get; set; }
        public double Weight { get; set; }
        //public int ItemCatagoryId { get; set; }
        //public ItemCategory ItemCategory { get; set; }
        public string AvailibiltyStatus { get; set; }
        public DateTime ExpiryDate { get; set; }
        public ICollection<VendItem> VendItems { get; set; }
        public ICollection<Sale> Sales { get; set; }
    }
}
