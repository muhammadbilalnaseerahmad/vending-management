namespace Web_App_VM_Management_System.Entities
{
    public class Employee : BaseEntity
    {
        public string Name { get; set; }        // Name of the employee
        public string PhoneNumber { get; set; } // Phone number of the employee
        public string EmailAddress { get; set; } // Email address of the employee
        public double Salary { get; set; }     // Salary of the employee
        public bool IsPaid { get; set; }        // Salary status (paid or unpaid)
    }
}
