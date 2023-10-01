using Microsoft.AspNetCore.Mvc;
using Seminar_2.Models;

namespace Seminar_2.Controllers
{
    public class HomeController : Controller
    { 
        public HomeController()
        {           
        }

        public IActionResult Index()
        {
            return View(Product.GetAll());
        }       
    }
}