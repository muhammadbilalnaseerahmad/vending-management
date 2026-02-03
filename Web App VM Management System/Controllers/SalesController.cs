using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_App_VM_Management_System.AppContext;
using Web_App_VM_Management_System.Entities;

namespace Web_App_VM_Management_System.Controllers
{
    public class SalesController : Controller
    {
        private readonly VMdbContext vmDBContext;

        public SalesController(VMdbContext vMdbContext)
        {
            this.vmDBContext = vMdbContext;
        }
        public IActionResult Index(string message = "", string color = "")
        {
            ViewBag.Message = message;
            ViewBag.Color = color;
            List<Sale> salesList = this.vmDBContext.Sale.Include(s => s.InventoryItem).Include(s=>s.Product).ToList();
            ViewBag.listItems = vmDBContext.InventoryItems.Where(x => x.QuantityInStock > 0).ToList();
            ViewBag.listMachines = vmDBContext.Products.Where(x => !x.IsSoldOut).ToList();
            return View(salesList);
        }
        public async Task<IActionResult> AddSale(Sale sale)
        {
            if (sale.ItemId <= 0 || sale.UnitSold <= 0)
            {
                return RedirectToAction("Index", new { message = "Invalid Request!", color = "red" });
            }
            int lastReading = vmDBContext.Sale.Sum(x=>x.UnitSold);
            if (lastReading >= sale.UnitSold)
            {
                return RedirectToAction("Index", new { message = "Invalid Reading!", color = "red" });
            }
            InventoryItem inventory = vmDBContext.InventoryItems.FirstOrDefault(x => x.Id == sale.ItemId);
            if (lastReading != 0)
            {
                sale.UnitSold = sale.UnitSold - lastReading;
            }
            sale.TotalSale = (int)(sale.UnitSold * inventory.Price);
            sale.Status = "Active";
            sale.CreatedDate = DateTime.Now;
            vmDBContext.Sale.Add(sale);
            vmDBContext.SaveChanges();
            return RedirectToAction("Index", new { message = "Sale added succesfully!", color = "green" });
        }

        [HttpGet]
        public IActionResult DeleteSale(int id)
        {
            // Retrieve the product with the given id from the database
            var itemToDelete = vmDBContext.Sale.Find(id);
            if (itemToDelete == null)
            {
                // Product not found, handle accordingly (e.g., return a not found view)
                return RedirectToAction("Index", new { message = "Record could not found!", color = "red" });
            }

            // Delete the product from the database
            vmDBContext.Sale.Remove(itemToDelete);
            vmDBContext.SaveChanges();
            // Redirect to the product list page or another appropriate action
            return RedirectToAction("Index", new { message = "Record deleted successfuly!", color = "green" });
        }
    }
}
