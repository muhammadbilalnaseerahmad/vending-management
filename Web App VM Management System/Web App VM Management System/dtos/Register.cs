using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Web_App_VM_Management_System.dtos
{
    public class Register
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
    }
}
