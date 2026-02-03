using Microsoft.AspNetCore.Mvc;
using System.Security.Permissions;
using Web_App_VM_Management_System.AppContext;
using Web_App_VM_Management_System.Entities;

namespace Web_App_VM_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly VMdbContext vMdbContext;
        public HomeController(VMdbContext vMdbContext)
        {
            this.vMdbContext = vMdbContext;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> listProducts = vMdbContext.Products.ToList();
            return View(listProducts);
        }
        
    }
}
