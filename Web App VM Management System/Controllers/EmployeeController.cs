using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Web_App_VM_Management_System.AppContext;
using Web_App_VM_Management_System.dtos;
using Web_App_VM_Management_System.Entities;

namespace Web_App_VM_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly VMdbContext _dbContext;

        public EmployeeController(VMdbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.vmCount = _dbContext.Products.Where(x => x.StockQuantity > 0).Count();
                ViewBag.invenCount = _dbContext.InventoryItems.Where(x => x.QuantityInStock > 0).Sum(x => x.QuantityInStock);
                ViewBag.catCount = _dbContext.Categories.Count();
                ViewBag.viCount = _dbContext.VendItems.Count();
                List<Product> products = _dbContext.Products.ToList();
                List<MachineSaleDTO> machines = new List<MachineSaleDTO>();
                foreach (var item in products)
                {
                    MachineSaleDTO dTO = new MachineSaleDTO()
                    {
                        Name = item.Name,
                        TotalSale = _dbContext.Sale.Where(s => s.ProductId == item.Id).Sum(s => s.TotalSale)
                    };
                    machines.Add(dTO);
                }
                ViewBag.todaySale = _dbContext.Sale.ToList().OrderBy(x => x.CreatedDate).LastOrDefault()?.TotalSale;
                ViewBag.totalSale = _dbContext.Sale.Sum(s => s.TotalSale);
                ViewBag.ii = _dbContext.InventoryItems.Where(x => x.QuantityInStock > 0).Sum(s => s.Price);
                ViewBag.vmList = products;
                return View(machines);
            }
            // The user is not logged in
            return RedirectToAction("Login", "Account"); // Redirect to a login page or take appropriate action

        }
        public IActionResult EmployeesList(string message = "", string color = "")
        {

            ViewBag.message = message;
            ViewBag.color = color;
            IEnumerable<Employee> employeesList = _dbContext.Employees.ToList();
            return View(employeesList);
        }

        [HttpGet]
        public IActionResult AddEmployee(string message = "", string color = "")
        {
            ViewBag.message = message;
            ViewBag.color = color;
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {

            // Perform any necessary validation and processing
            employee.IsPaid = false;
            employee.Status = "Active";
            employee.CreatedDate = DateTime.Now;
            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();
            return RedirectToAction("EmployeesList", new { message = "Employee added successfuly!", color = "green" });
        }

        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            Employee employee = _dbContext.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        public IActionResult EditEmployee(Employee editedEmployee)
        {
            Employee originalEmployee = _dbContext.Employees.FirstOrDefault(e => e.Id == editedEmployee.Id);
            if (originalEmployee == null)
            {
                return NotFound();
            }
            // Perform any necessary validation and processing
            originalEmployee.Name = editedEmployee.Name;
            originalEmployee.PhoneNumber = editedEmployee.PhoneNumber;
            originalEmployee.EmailAddress = editedEmployee.EmailAddress;
            originalEmployee.Salary = editedEmployee.Salary;
            originalEmployee.IsPaid = editedEmployee.IsPaid;
            originalEmployee.ModifiedDate = DateTime.Now;

            _dbContext.SaveChanges();
            return Redirect("/Admin/Index");
        }

        [HttpGet]
        public IActionResult DeleteEmployee(int id)
        {
            Employee employeeToDelete = _dbContext.Employees.FirstOrDefault(e => e.Id == id);
            if (employeeToDelete == null)
            {
                return RedirectToAction("EmployeesList", new { message = "Record does not exist!", color = "red" });
            }
            _dbContext.Employees.Remove(employeeToDelete);
            _dbContext.SaveChanges();
            return RedirectToAction("EmployeesList", new { message = "Employee deleted successfuly!", color = "green" });
        }
        [HttpGet]
        public async Task<object> UpdateSalaryStatus(int id, bool isChecked)
        {
            Employee employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
            employee.IsPaid = isChecked;
            if (employee == null)
            {
                return RedirectToAction("EmployeesList", new { message = "Record does not exist!", color = "red" });
            }
            _dbContext.Employees.Update(employee);
            _dbContext.SaveChanges();
            string status = isChecked ? "Paid" : "Unpaid";
            string color = isChecked ? "green" : "red";
            return RedirectToAction("EmployeesList", new { message = $"Salary status set to {status}!", color = color });
        }


    }
}
