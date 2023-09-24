using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Seminar_1.Models.Entities;
using Seminar_1.Models.VMs;

namespace Seminar_1.Controllers
{
    [Route("[Controller]")]
    public class ProductController : Controller
    {
        private const string imgFolderName = "img";
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly Seminar1Context context;

        public ProductController(IWebHostEnvironment hostEnvironment, Seminar1Context context)
        {      
            this.hostEnvironment= hostEnvironment;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = context.Products.Select(p => new ProductVM().ProdToProdVM(p)).ToList();
            return View(list);
        }

        [HttpGet]
        [Route("New")]
        public IActionResult New()
        {
            var product = new ProductVM();
            return View(product);
        }

        [HttpPost]
        [Route("New")]
        public IActionResult Create(ProductVM dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "There were some errors in your form");           
                return View("New", dto);
            }

            SaveImage(dto);
            context.Add(ProductVM.VMProdToProd(dto));
            context.SaveChanges();

            return View("Index", context.Products.Select(p => new ProductVM().ProdToProdVM(p)).ToList());
        }

        //[HttpGet]
        //[Route("Edit/{id}")]
        //public IActionResult Edit(int id)
        //{
        //    var dto = service.GetProduct(id);
        //    dto.ProductTypes = service.GetProductTypes();
        //    return View(dto);
        //}

        //[HttpPost]
        //[Route("Edit/{id}")]
        //public IActionResult Edit(int id, ProductVM dto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError(string.Empty, "There were some errors in your form");
        //        dto.ProductTypes = service.GetProductTypes();
        //        return View(dto);
        //    }

        //    service.UpdateProduct(id, dto);

        //    return View("Index", service.GetAllProducts());
        //}

        //[HttpDelete]
        //[Route("Delete/{id}")]
        //public JsonResult Delete(int id)
        //{
        //    service.DeleteProduct(id);
        //    return Json(new { success = true, message = "Delete success" });
        //}

        private void SaveImage(ProductVM dto)
        {
            if (dto.ProducImage == null)
                return;

            var imgFolderPath = Path.Combine(hostEnvironment.WebRootPath, imgFolderName);

            if (!Directory.Exists(imgFolderPath))
                Directory.CreateDirectory(imgFolderPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(dto.ProducImage.FileName);
            var imgFullPath = Path.Combine(imgFolderPath, fileName);

            using (var fileStream = new FileStream(imgFullPath, FileMode.Create))
                dto.ProducImage.CopyTo(fileStream);

            dto.ImagePath = Path.Combine(imgFolderName, fileName);
        }         
    }
}
