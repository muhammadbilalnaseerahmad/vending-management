using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Web_App_VM_Management_System.AppContext;
using Web_App_VM_Management_System.Entities;

namespace Web_App_VM_Management_System.Controllers
{
    public class CategoryController : Controller
    {
        private readonly VMdbContext _vMdbContext;

        public CategoryController(VMdbContext vMdbContext)
        {
            _vMdbContext = vMdbContext;
        }

        public IActionResult CategoriesList(string message = "", string color = "")
        {
            ViewBag.message = message;
            ViewBag.color = color;
            IEnumerable<Category> categoriesList = _vMdbContext.Categories.ToList();
            return View(categoriesList);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            category.CreatedDate = DateTime.Now;
            category.Status = "Active";
            _vMdbContext.Categories.Add(category);
            _vMdbContext.SaveChanges();
            return RedirectToAction("CategoriesList", new { message = "Category added successfuly!", color = "green" });
        }

        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            Category category = _vMdbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategory(Category editedCategory)
        {
            Category originalCategory = _vMdbContext.Categories.FirstOrDefault(c => c.Id == editedCategory.Id);
            originalCategory.Name = editedCategory.Name;
            originalCategory.Description = editedCategory.Description;
            _vMdbContext.SaveChanges();
            return RedirectToAction("CategoriesList");
        }

        [HttpGet]
        public IActionResult DeleteCategory(int id)
        {
            Category categoryToDelete = _vMdbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryToDelete == null)
            {
                return NotFound();
            }
            _vMdbContext.Categories.Remove(categoryToDelete);
            _vMdbContext.SaveChanges();
            return RedirectToAction("CategoriesList");
        }
    }
}
