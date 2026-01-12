using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulation1MPA201.Contexts;
using Simulation1MPA201.Helpers;
using Simulation1MPA201.Models;
using Simulation1MPA201.ViewModels.Product;

namespace Simulation1MPA201.Areas.Admin.Controllers;
[Area("Admin")]
//[Authorize(Roles ="Admin")]
public class ProductController : Controller
{
    private readonly SimulationDbContext _context;
    private IWebHostEnvironment _environment;
    private readonly string folderPath;

    public ProductController(SimulationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.Select(x => new ProductGetVM()
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            Price = x.Price,
            Rating = x.Rating,
            CategoryName = x.Category.Name
        }).ToListAsync();


        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        await SendCategoriesWithViewBag();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateVM vm)
    {
        await SendCategoriesWithViewBag();
        if (!ModelState.IsValid)
        {
            await SendCategoriesWithViewBag();
            return View(vm);
        }

        var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
        if (!isExistCategory)
        {
            return BadRequest();
        }

        if (!vm.Image.CheckSize(2))
        {
            ModelState.AddModelError("", "Max 2 mb");
            return View(vm);
        }

        if (!vm.Image.CheckType("image"))
        {
            ModelState.AddModelError("", "Image must be in correct form");
            return View(vm);
        }

        var uniqueFileName = await vm.Image.UploadFileAsync(folderPath);

        Product newProduct = new Product()
        {
            Title = vm.Title,
            Description = vm.Description,
            Price = vm.Price,
            Rating = vm.Rating,
            ImagePath = uniqueFileName,
            CategoryId=vm.CategoryId
        };

        await _context.Products.AddAsync(newProduct);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        string deletedPath = Path.Combine(folderPath, product.ImagePath);

        FileHelper.DeleteFile(deletedPath);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        ProductUpdateVM vm = new ProductUpdateVM()
        {
            Id = product.Id,
            Description = product.Description,
            Rating = product.Rating,
            Price = product.Price,
            CategoryId = product.CategoryId,
            Title = product.Title
        };

        await SendCategoriesWithViewBag();

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProductUpdateVM vm)
    {
        await SendCategoriesWithViewBag();
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var existProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == vm.Id);
        if (existProduct == null)
        {
            return NotFound();
        }

        var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
        if (!isExistCategory)
        {
            return BadRequest();
        }

        if (vm.Image?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("", "Max 2 mb");
            return View(vm);
        }

        if (vm.Image?.CheckType("image") ?? false)
        {
            ModelState.AddModelError("", "Image must be in correct form");
            return View(vm);
        }

        existProduct.Title = vm.Title;
        existProduct.Description = vm.Description;
        existProduct.Price = vm.Price;
        existProduct.Rating = vm.Rating;
        existProduct.CategoryId = vm.CategoryId;

        if(vm.Image is { })
        {
            var fileName = await vm.Image.UploadFileAsync(folderPath);
            string deletedPath = Path.Combine(folderPath, existProduct.ImagePath);

            FileHelper.DeleteFile(deletedPath);

            existProduct.ImagePath = fileName;

        }

         _context.Products.Update(existProduct);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));


    }

    private async Task SendCategoriesWithViewBag()
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
    }

}
