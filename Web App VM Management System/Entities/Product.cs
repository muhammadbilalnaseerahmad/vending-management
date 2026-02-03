namespace Web_App_VM_Management_System.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }     // Name of the product
        public string Description { get; set; }  // Description of the product
        public decimal Price { get; set; }    // Price of the product
        public int StockQuantity { get; set; } // Available quantity in stock
        //public int Catagoryid { get; set; }
        public int NumbeofFloors { get; set; }
        public int EachFloorCapacity { get; set; }
        public bool IsSoldOut { get; set; }
        public string OwnerContact { get; set; }
        //public Category ProductCategory { get; set; } // Category the product belongs to
        public string Image { get; set; } // Category the product belongs to
        public ICollection<VendItem> VendItems { get; set;}
        public ICollection<Sale> Sales { get; set;}
    }
}
