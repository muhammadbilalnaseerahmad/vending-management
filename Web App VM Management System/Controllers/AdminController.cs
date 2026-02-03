using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Web_App_VM_Management_System.AppContext;
using Microsoft.EntityFrameworkCore;
using Web_App_VM_Management_System.Entities;

namespace Web_App_VM_Management_System.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        VMdbContext VMdbContext { get; set; }
        public AdminController(VMdbContext vMdbContext)
        {
            VMdbContext = vMdbContext;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.vmCount = VMdbContext.Products.Where(x => x.StockQuantity > 0).Count();
                ViewBag.invenCount = VMdbContext.InventoryItems.Where(x => x.QuantityInStock > 0).Sum(x => x.QuantityInStock);
                ViewBag.catCount = VMdbContext.Categories.Count();
                ViewBag.viCount = VMdbContext.VendItems.Count();
                List<Product> products = VMdbContext.Products/*.Include(p => p.ProductCategory)*/.ToList();
                ViewBag.vmList = products;
                //ViewBag.ruCount = VMdbContext.Employees.Count();
                return View();
            }
            // The user is not logged in
            return RedirectToAction("Login", "Account"); // Redirect to a login page or take appropriate action

        }
    }
}
