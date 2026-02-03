using Microsoft.AspNetCore.Mvc;

namespace Web_App_VM_Management_System.Controllers
{
    public class ProductGridController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
