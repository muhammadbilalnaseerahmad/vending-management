using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Web_App_VM_Management_System.AppContext;
using Web_App_VM_Management_System.Entities;

namespace Web_App_VM_Management_System.Controllers
{
    public class InventoryItemsController : Controller
    {
        private readonly VMdbContext vMdbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public InventoryItemsController(VMdbContext vMdbContext, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            this.vMdbContext = vMdbContext;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        public IActionResult InventoryItemsList(string message = "", string color = "")
        {
            ViewBag.Message = message;
            ViewBag.Color = color;
            ViewBag.vmCount = vMdbContext.InventoryItems.Count();
            ViewBag.catCount = vMdbContext.Categories.Count();
            ViewBag.empCount = vMdbContext.Employees.Count();
            List<InventoryItem> InventoryItems = vMdbContext.InventoryItems.Where(ii => ii.QuantityInStock > 0)/*.Include(p => p.ItemCategory)*/.ToList();
            return View(InventoryItems);
        }
        [HttpGet]
        public IActionResult AddInventoryItem()
        {
            ViewBag.catagories = vMdbContext.ItemCategories.ToList();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditInventoryItem(int id)
        {
            var inventoryItem=await vMdbContext.InventoryItems.FindAsync(id);
            ViewBag.catagories = vMdbContext.ItemCategories.ToList();
            return View(inventoryItem);
        }
        [HttpPost]
        public async Task<IActionResult> EditInventoryItem(InventoryItem obj)
        {
            var item = await vMdbContext.InventoryItems.FindAsync(obj.Id);
            item.Name=obj.Name;
            item.Description=obj.Description;
            item.Price=obj.Price;
            item.QuantityInStock=obj.QuantityInStock;
            item.Weight=obj.Weight;
            vMdbContext.SaveChanges();
            return RedirectToAction("InventoryItemsList", new { message = "Updated Successfully!", color = "green" });
        }
        [HttpPost]
        public IActionResult AddInventoryItem(InventoryItem item)
        {

            item.CreatedDate = DateTime.Now;
            item.AvailibiltyStatus = "Available";
            item.Status = "Active";
            vMdbContext.InventoryItems.Add(item);
            vMdbContext.SaveChanges();
            return RedirectToAction("InventoryItemsList", new { message = "Successfully Created!", color = "green" });
        }
        //[HttpGet]
        //public IActionResult EditInventoryItem(int id)
        //{
        //    // Retrieve the InventoryItem with the given id from the database
        //    InventoryItem InventoryItem = vMdbContext.InventoryItems.FirstOrDefault(p => p.Id == id);
        //    Console.WriteLine(InventoryItem);

        //    if (InventoryItem == null)
        //    {
        //        // InventoryItem not found, handle accordingly (e.g., return a not found view)
        //        return NotFound();
        //    }
        //    ViewBag.catagories = vMdbContext.Categories.ToList();
        //    return View(InventoryItem);
        //}

        //[HttpPost]
        //public IActionResult EditInventoryItem(InventoryItem editedInventoryItem)
        //{
        //    InventoryItem originalInventoryItem = vMdbContext.InventoryItems.FirstOrDefault(p => p.Id == editedInventoryItem.Id);
        //    originalInventoryItem.Name = editedInventoryItem.Name;
        //    originalInventoryItem.Description = editedInventoryItem.Description;
        //    originalInventoryItem.Price = editedInventoryItem.Price;
        //    originalInventoryItem.StockQuantity = editedInventoryItem.StockQuantity;
        //    originalInventoryItem.Catagoryid = editedInventoryItem.Catagoryid;
        //    originalInventoryItem.ModifiedDate = DateTime.Now;
        //    //InventoryItem.Status = "Active";
        //    //vMdbContext.InventoryItems.Update(InventoryItem);
        //    vMdbContext.SaveChanges();
        //    return Redirect("/Admin/Index");
        //}


        [HttpGet]
        public async Task<IActionResult> DeleteInventoryItem(int id)
        {
            try
            {
                var inventoryItemToDelete = await vMdbContext.InventoryItems.FindAsync(id);
                if (inventoryItemToDelete == null)
                {
                    return RedirectToAction("InventoryItemsList", new { message = "Record does not exist!", color = "red" });
                }
                vMdbContext.InventoryItems.Remove(inventoryItemToDelete);
                await vMdbContext.SaveChangesAsync();
                return RedirectToAction("InventoryItemsList", new { message = "Deleted Successfuly!", color = "green" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("InventoryItemsList", new { message = $"Error -{ex.Message}!", color = "red" });
            }
        }
    }
}
