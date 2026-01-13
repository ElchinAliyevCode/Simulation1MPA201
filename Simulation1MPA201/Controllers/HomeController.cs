using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulation1MPA201.Contexts;
using Simulation1MPA201.ViewModels.Product;

namespace Simulation1MPA201.Controllers
{
    public class HomeController : Controller
    {
        private readonly SimulationDbContext _context;

        public HomeController(SimulationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products=await _context.Products.Select(x=>new ProductGetVM()
            {
                Description = x.Description,
                CategoryName=x.Category.Name,
                Price = x.Price,
                Rating = x.Rating,
                Title = x.Title,
                ImagePath = x.ImagePath,
            }).ToListAsync();
            return View(products);
        }


    }
}
