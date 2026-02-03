namespace Web_App_VM_Management_System.dtos
{
    public class VendItemDTO
    {
        public int Id { get; set; }
        public string VendingMachineName { get; set; }
        public int ItemsCount { get;set; }
        public int TrayNumber { get; set; }
        public string TrayItemsList { get; set; }
        public bool isFloorCapacityFull { get; set; }
    }
}
