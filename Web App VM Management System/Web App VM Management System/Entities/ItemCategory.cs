namespace Web_App_VM_Management_System.Entities
{
    public class ItemCategory:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<InventoryItem> InventoryItems { get; set; }
    }
}
