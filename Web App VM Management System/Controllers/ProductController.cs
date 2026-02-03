using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using Web_App_VM_Management_System.AppContext;
using Web_App_VM_Management_System.dtos;
using Web_App_VM_Management_System.Entities;

namespace Web_App_VM_Management_System.Controllers
{
    public class ProductController : Controller
    {
        private readonly VMdbContext vMdbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        public ProductController(VMdbContext vMdbContext, IWebHostEnvironment hostingEnvironment, IConfiguration configuration, UserManager<IdentityUser> _userManager)
        {
            this.vMdbContext = vMdbContext;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            this._userManager = _userManager;
        }
        public IActionResult ProductsList(string message = "", string color = "")
        {
            ViewBag.Message = message;
            ViewBag.Color = color;
            ViewBag.vmCount = vMdbContext.Products.Count();
            ViewBag.catCount = vMdbContext.Categories.Count();
            ViewBag.empCount = vMdbContext.Employees.Count();
            List<Product> products = vMdbContext.Products./*Include(p => p.ProductCategory).*/ToList();
            ViewBag.vmList = products;
            //ViewBag.ruCount = VMdbContext.Employees.Count();
            return View();
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            ViewBag.catagories = vMdbContext.Categories.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDTO dto)
        {
            var user = await _userManager.GetUserAsync(User);
            Product product = new Product()
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                //Catagoryid=dto.Catagoryid,
                NumbeofFloors = dto.NumbeofFloors,
                EachFloorCapacity = dto.EachFloorCapacity,
                OwnerContact = user.PhoneNumber,
                IsSoldOut = false,
            };
            if (dto.ImageFile != null)
            {
                var fileName = Path.GetFileName(dto.ImageFile.FileName);

                // Construct the file path in the wwwroot directory
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", fileName);

                // Save the file to the wwwroot directory
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    dto.ImageFile.CopyTo(stream);
                }

                // Store the relative file path in the database
                var relativeFilePath = "Uploads/" + fileName; // Relative to wwwroot
                product.Image = _configuration.GetSection("ApplicationURL").Value + relativeFilePath;
            }
            else
            {
                product.Image = "";
            }
            product.CreatedDate = DateTime.Now;
            product.Status = "Active";
            vMdbContext.Products.Add(product);
            vMdbContext.SaveChanges();
            return RedirectToAction("ProductsList", new { message = "Successfully Created!", color = "green" });
        }
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            // Retrieve the product with the given id from the database
            Product product = vMdbContext.Products.FirstOrDefault(p => p.Id == id);
            Console.WriteLine(product);

            if (product == null)
            {
                // Product not found, handle accordingly (e.g., return a not found view)
                return NotFound();
            }
            ViewBag.catagories = vMdbContext.Categories.ToList();
            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product editedProduct)
        {
            Product originalProduct = vMdbContext.Products.FirstOrDefault(p => p.Id == editedProduct.Id);
            originalProduct.Name = editedProduct.Name;
            originalProduct.Description = editedProduct.Description;
            originalProduct.Price = editedProduct.Price;
            originalProduct.StockQuantity = editedProduct.StockQuantity;
            //originalProduct.Catagoryid = editedProduct.Catagoryid;
            originalProduct.ModifiedDate = DateTime.Now;
            //product.Status = "Active";
            //vMdbContext.Products.Update(product);
            vMdbContext.SaveChanges();
            return Redirect("/Admin/Index");
        }


        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            // Retrieve the product with the given id from the database
            var productToDelete = vMdbContext.Products.FirstOrDefault(p => p.Id == id);
            if (productToDelete == null)
            {
                // Product not found, handle accordingly (e.g., return a not found view)
                return RedirectToAction("ProductsList", new { message = "Record could not found!", color = "red" });
            }
            // Delete the product from the database
            vMdbContext.Products.Remove(productToDelete);
            vMdbContext.SaveChanges();
            // Redirect to the product list page or another appropriate action
            return RedirectToAction("ProductsList", new { message = "Record deleted successfuly!", color = "green" });
        }
    }
}
