using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulation1MPA201.Contexts;
using Simulation1MPA201.ViewModels.Product;

namespace Simulation1MPA201.Areas.Admin.Controllers;

public class ProductController : Controller
{
    private readonly SimulationDbContext _context;

    public ProductController(SimulationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.Select(x => new ProductGetVM()
        {
            Title=x.Title,
            Description=x.Description,
            Price=x.Price,
            Rating=x.Rating,
            CategoryName=x.Category.Name
        }).ToListAsync();
        return View();
    }
}
