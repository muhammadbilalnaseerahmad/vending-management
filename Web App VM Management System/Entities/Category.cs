namespace Web_App_VM_Management_System.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }     // Name of the category
        public string Description { get; set; } // Description of the categor
        public ICollection<Product> Products { get; set; }
        //public ICollection<Product> Products { get; set; }
    }
}
