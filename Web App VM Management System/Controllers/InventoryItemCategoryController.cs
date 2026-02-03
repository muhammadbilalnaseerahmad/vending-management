using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using Web_App_VM_Management_System.AppContext;
using Web_App_VM_Management_System.Entities;

namespace Web_App_VM_Management_System.Controllers
{
    public class InventoryItemCategoryController : Controller
    {
        private readonly VMdbContext _vMdbContext;

        public InventoryItemCategoryController(VMdbContext vMdbContext)
        {
            _vMdbContext = vMdbContext;
        }
        [HttpPost]
        public IActionResult AddItemCategory(ItemCategory category)
        {
            category.CreatedDate = DateTime.Now;
            category.Status = "Active";
            _vMdbContext.ItemCategories.Add(category);
            _vMdbContext.SaveChanges();
            return RedirectToAction("ItemCategoriesList", new { message = "Category added successfuly!", color = "green" });
        }

        [HttpGet]
        public IActionResult ItemCategoriesList(string message="", string color="")
        {
            ViewBag.message = message;
            ViewBag.color = color;
            IEnumerable<ItemCategory> categoriesList = _vMdbContext.ItemCategories.ToList();
            return View(categoriesList);
        }
        [HttpGet]
        public IActionResult AddItemCategory()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DeleteIneventoryItemCategory(int id)
        {
            ItemCategory categoryToDelete = _vMdbContext.ItemCategories.FirstOrDefault(c => c.Id == id);
            if (categoryToDelete == null)
            {
                return RedirectToAction("ItemCategoriesList", new { messsage = "Record not found!", color = "red" });
            }
            _vMdbContext.ItemCategories.Remove(categoryToDelete);
            _vMdbContext.SaveChanges();
            return RedirectToAction("ItemCategoriesList",new {messsage="Record deleted Successfuly!",color="green"});
        }
    }
}
