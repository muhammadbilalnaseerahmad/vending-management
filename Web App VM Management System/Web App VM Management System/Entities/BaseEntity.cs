using System.ComponentModel.DataAnnotations;

namespace Web_App_VM_Management_System.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public string Status { get; set; }
    }
}
