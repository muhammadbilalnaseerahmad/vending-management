using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Web_App_VM_Management_System.AppContext;
using Web_App_VM_Management_System.dtos;
using Web_App_VM_Management_System.Entities;

namespace Web_App_VM_Management_System.Controllers
{
    public class VendItemController : Controller
    {
        private readonly VMdbContext vMdbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public VendItemController(VMdbContext vMdbContext, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            this.vMdbContext = vMdbContext;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult VendItemList(string message = "", string color = "")
        {
            IEnumerable<VendItem> vendItems = vMdbContext.VendItems.Count() > 0 ? vMdbContext.VendItems.Include(vi => vi.Product).Include(vi => vi.InventoryItem).ToList() : new List<VendItem>();
            List<VendItemDTO> listDTos = new List<VendItemDTO>();
            foreach (var item in vendItems)
            {
                VendItemDTO obj = new VendItemDTO()
                {
                    Id = item.Id,
                    VendingMachineName = item.Product.Name,
                    ItemsCount = vendItems.Where(x => x.VendingMachineId == item.VendingMachineId).Count()
                };
                if (listDTos.Where(x => x.VendingMachineName == obj.VendingMachineName).Count() == 0)
                {
                    listDTos.Add(obj);
                }
            }
            ViewBag.vmList = vMdbContext.Products.Where(p => p.StockQuantity > 0).ToList();
            ViewBag.iItemsList = vMdbContext.InventoryItems.Where(i => i.QuantityInStock > 0).ToList();
            //ViewBag.vendingMachineId = vendingMachineId;
            ViewBag.message = message;
            ViewBag.color = color;
            return View(listDTos);
        }
        [HttpGet]
        public async Task<object> GetVendingProductsByMachineId(int id)
        {
            List<InventoryItem> machineItems = new List<InventoryItem>();
            List<VendItem> vendItems = vMdbContext.VendItems.Include(v => v.Product).Include(v => v.InventoryItem).Where(v => v.Product.Id == id).ToList();
            foreach (var item in vendItems)
            {
                InventoryItem inventoty = new InventoryItem()
                {
                    Id = item.InventoryItemId,
                    Name = item.InventoryItem.Name,
                    QuantityInStock = item.InventoryItem.QuantityInStock,
                };
                if (!(machineItems.Where(mi => mi.Name == inventoty.Name).Count() > 0))
                {
                    machineItems.Add(inventoty);
                }
            }
            return new
            {
                StatusCode = 200,
                result = machineItems
            };
        }

        [HttpGet]
        public async Task<object> GetVendingItemsByMachineName(string vendingMachineName)
        {
            List<VendItem> vendItems = vMdbContext.VendItems.Include(v => v.Product).Include(v => v.InventoryItem).Where(v => v.Product.Name == vendingMachineName).ToList();
            List<VendItemDTO> vendItemDTOs = new List<VendItemDTO>();

            foreach (var item in vendItems)
            {
                int totalTrayItems = vendItems.Where(x => x.FloorNumber == item.FloorNumber).Count();
                var groupedItems = vendItems.Where(x => x.FloorNumber == item.FloorNumber).GroupBy(x => x.InventoryItem.Name);
                VendItemDTO obj = new VendItemDTO()
                {
                    TrayNumber = item.FloorNumber,
                    TrayItemsList = string.Join(", ", groupedItems.Select(group =>
                    {
                        if (group.Count() > 1)
                        {
                            return $"{group.Key} ({group.Count()})";
                        }
                        else
                        {
                            return group.Key;
                        }
                    })),
                };
                if (vendItemDTOs.Where(x => x.TrayNumber == obj.TrayNumber).Count() == 0)
                {
                    obj.isFloorCapacityFull = item.Product.EachFloorCapacity == totalTrayItems ? true : false;
                    vendItemDTOs.Add(obj);
                }
            }
            return vendItemDTOs.OrderBy(x => x.TrayNumber);
        }
        [HttpGet]
        public IActionResult AddVendItem(string message = "", string color = "", int? vendingMachineId = null)
        {
            ViewBag.vmList = vMdbContext.Products.Where(p => p.StockQuantity > 0).ToList();
            ViewBag.iItemsList = vMdbContext.InventoryItems.Where(i => i.QuantityInStock > 0).ToList();
            ViewBag.vendingMachineId = vendingMachineId;
            ViewBag.message = message;
            ViewBag.color = color;
            return View();
        }
        [HttpGet]
        public async Task<Object> GetFloorsbyMachineId(int machineId)
        {
            try
            {
                int nOofFloors = (await vMdbContext.Products.FindAsync(machineId)).NumbeofFloors;
                var iItemsList = vMdbContext.InventoryItems.ToList();
                return new
                {
                    status = 200,
                    nOofFloors,
                    iItemsList
                };
                //return View();
            }
            catch (Exception e)
            {
                return new
                {
                    status = HttpStatusCode.InternalServerError,
                    error = e.Message
                };
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddVendItem(VendItem vendItem)
        {
            if (vendItem.FloorNumber == 0)
            {
                return RedirectToAction("VendItemList", new { message = "Floor number must be provided!", color = "red" });
            }
            InventoryItem item = await vMdbContext.InventoryItems.FindAsync(vendItem.InventoryItemId);
            if (vendItem.ItemCount > item.QuantityInStock)
            {
                return RedirectToAction("VendItemList", new { message = "Insufficiant Stock!", color = "red" });
            }
            Product vending = await vMdbContext.Products.FindAsync(vendItem.VendingMachineId);
            int totalFloorItems = vMdbContext.VendItems.Where(vi => vi.FloorNumber == vendItem.FloorNumber && vi.VendingMachineId == vendItem.VendingMachineId).Count();
            if (totalFloorItems + vendItem.ItemCount > vending.EachFloorCapacity)
            {
                return RedirectToAction("VendItemList", new { message = $"Floor {vendItem.FloorNumber} capacity exceeds.Max limit of vending machine `{vending.Name}` is {vending.EachFloorCapacity}", color = "red" });
            }
            for (int i = 0; i < vendItem.ItemCount; i++)
            {
                VendItem obj = new VendItem()
                {
                    Product = vendItem.Product,
                    VendingMachineId = vendItem.VendingMachineId,
                    InventoryItem = vendItem.InventoryItem,
                    InventoryItemId = vendItem.InventoryItemId,
                    FloorNumber = vendItem.FloorNumber,
                    ItemCount = vendItem.ItemCount,
                    Status = "Active",
                    CreatedDate = DateTime.Now,
                };
                await vMdbContext.VendItems.AddAsync(obj);
            }
            InventoryItem invenItem = await vMdbContext.InventoryItems.FindAsync(vendItem.InventoryItemId);
            invenItem.QuantityInStock -= vendItem.ItemCount;
            await vMdbContext.SaveChangesAsync();
            return RedirectToAction("VendItemList", new { message = "Item added successfuly!", color = "green" });
        }

        [HttpGet]
        public IActionResult DeleteVendItem(string name)
        {
            // Retrieve the product with the given id from the database
            var itemToDelete = vMdbContext.VendItems.Include(x => x.Product).FirstOrDefault(p => p.Product.Name == name);
            if (itemToDelete == null)
            {
                // Product not found, handle accordingly (e.g., return a not found view)
                return RedirectToAction("VendItemList", new { message = "Record could not found!", color = "red" });
            }

            // Delete the product from the database
            vMdbContext.VendItems.Remove(itemToDelete);
            vMdbContext.SaveChanges();
            // Redirect to the product list page or another appropriate action
            return RedirectToAction("VendItemList", new { message = "Record deleted successfuly!", color = "green" });
        }
    }
}
